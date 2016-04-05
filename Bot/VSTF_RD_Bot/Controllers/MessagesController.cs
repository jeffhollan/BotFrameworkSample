using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using VSTF_RD_Bot.Models;
using System.Collections.Generic;
using VSTF_RD_Bot.Controllers;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace VSTF_RD_Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private ConnectorClient client = new ConnectorClient();
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            #region Check Authorization
            string d = (string)message.BotUserData;
            if (d != "authorized")
            {
                return message.CreateReplyMessage($"You must authenticate to use bot: https://jehollanVSBot.azurewebsites.net/api/{message.From.Id}/login");
            }
            #endregion

            if (message.Type == "Message")
            {
                #region bot /commands
                if (Regex.IsMatch(message.Text, Constants.regex_command))
                {
                    string command = Regex.Match(message.Text, Constants.regex_command).Groups[1].Value;
                    switch(command.ToLower())
                    {
                        case "create":
                            return createCommand(message);
                        case "topfeatures":
                            return logicAppCommand(message, "TopFeatures");
                        case "topbugs":
                            return logicAppCommand(message, "TopBugs");
                        case "topcri":
                            return logicAppCommand(message, "TopCRI");
                        case "currentsprint":
                            return logicAppCommand(message, "CurrentSprint");
                        case "start":
                            return message.CreateReplyMessage("Please welcome your bot overlord", "en");
                        case "logout":
                            var getData = await client.Bots.GetUserDataAsync(Constants.botId, message.From.Id);
                            getData.Data = null;
                            await client.Bots.SetUserDataAsync(Constants.botId, message.From.Id, getData);
                            return message.CreateReplyMessage("You have been logged out");
                        default:
                            return message.CreateReplyMessage("Sorry, that's an invalid command", "en");
                    }
                    
                }
                #endregion
                else
                {
                    var reply = await Conversation.SendAsync(message, () => new VSTFDialog());

                    #region Check if I need to go start up a query or create
                    var dialogQuery = reply.GetBotConversationData<QueryItem>("userQuery");
                    var dialogCreate = reply.GetBotConversationData<TFSItem>("userItem");
                    if (dialogQuery != null)
                    {
                        reply.SetBotConversationData("userQuery", null);
                        Task.Factory.StartNew(async () =>
                        {
                            await getQueryResults(message, dialogQuery);
                        });

                    }
                    else if (dialogCreate != null)
                    {
                        reply.SetBotConversationData("userItem", null);
                        Task.Factory.StartNew(() =>
                        {
                            createTFSItem(message, dialogCreate);
                        });
                    }

                    #endregion

                    return reply;
                }
            
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        /// <summary>
        /// Executing a command will trigger a Logic App which will read the blob specified and execute the query, returning results
        /// </summary>
        /// <param name="message"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        private Message logicAppCommand(Message message, string blobName)
        {
            string reply = $"Executing query...";
            Task.Factory.StartNew(() =>
            {
                fetchCommand(message, new Command() { blobName = blobName });
            });
            return message.CreateReplyMessage(reply, "en");
        }

        private async void fetchCommand(Message message, Command command)
        {
            var response = await new LogicAppController().ExecuteCommand(command);
            IEnumerable<TFSResponse> items = await response.Content.ReadAsAsync<IEnumerable<TFSResponse>>();
            if (items.Count() == 0)
                client.Messages.SendMessage(message.CreateReplyMessage("No results found"));
            string text = "";
            foreach (var item in items)
            {
                text += $"{item.id}: {item.Title}\n\n";
            }
            var reply = message.CreateReplyMessage(text, "en");
            client.Messages.SendMessageAsync(reply);
        }

        private Message createCommand(Message message)
        {
            string type, title, text;
            text = message.Text;

            var match = System.Text.RegularExpressions.Regex.Match(text, Constants.regex_create);
            type = match.Groups[1].Value;
            title = match.Groups[2].Value;

            string reply = $"Creating new {type} with title {title}";
            var dialogCreate = new TFSItem() { title = title, type = type };
            Task.Factory.StartNew(() =>
            {
                createTFSItem(message, dialogCreate);
            });


            return message.CreateReplyMessage(reply, "en");
        }

        private async void createTFSItem(Message message, TFSItem dialogCreate)
        {
            var response = await new LogicAppController().CreateVSTF(dialogCreate);
            string itemId = (string)JObject.Parse(await response.Content.ReadAsStringAsync())["id"];
            await client.Messages.SendMessageAsync(message.CreateReplyMessage($"Created item {itemId}", "en"));
        }

        private async Task getQueryResults(Message message, QueryItem queryItem)
        {
                var response = await new LogicAppController().SearchVSTF(queryItem);
                IEnumerable<TFSResponse> items = await response.Content.ReadAsAsync<IEnumerable<TFSResponse>>();
                if (items.Count() == 0)
                    client.Messages.SendMessage(message.CreateReplyMessage("No results found"));
                foreach (var item in items)
                {
                    string text = $"{item.id}: {item.Title}";
                    var reply = message.CreateReplyMessage(text, "en");
                    client.Messages.SendMessageAsync(reply);
                    
                }
            

            
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}