using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VSTF_RD_Bot.Controllers;
using VSTF_RD_Bot.Models;
using System.Net.Http;
using System.Configuration;

namespace VSTF_RD_Bot
{
    [Serializable]
    [LuisModel(Constants.LUIS_AppId, Constants.LUIS_AppSecret)]
    public class VSTFDialog : LuisDialog
    {
        [LuisIntent("SearchVSTF")]
        public async Task searchVSTF(IDialogContext context, LuisResult result)
        {
            
            string query, type;
            if (TryFindType(result, out query, out type))
            {
                string message = $"Searching for {query} in {type}....";

                context.ConversationData.SetValue<QueryItem>("userQuery", new QueryItem() { query = query, type = type });

                await context.PostAsync(message);


                context.Wait(MessageReceived);

            }
            else if(query == null && type == null)
            {
                string message = $"I think you want to search, please try again.\n\n" + 
                    "Try a phrase like \"Search backlog for swagger\"";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else if(query == null)
            {
                string message = $"Ok, you want to search {type} - what should I search for?";
                context.ConversationData.SetValue("type", type);
                await context.PostAsync(message);
                context.Wait(GetOtherField);
            }
            else
            {

                string message = $"Ok, you want to search for items about {query} - what item type?";
                context.ConversationData.SetValue("query", query);
                await context.PostAsync(message);
                context.Wait(GetOtherField);
                
            }

        }

        [LuisIntent("AddVSTF")]
        public async Task addVSTF(IDialogContext context, LuisResult result)
        {
            string title, type;
            if (TryFindType(result, out title, out type))
            {
                string message = $"Creating new {type} with title {title}";

                new LogicAppController().CreateVSTF(new TFSItem() { title = title, type = type });
                
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            else
            {
                string message = $"I think you want to create an item but didn't catch the title or type.\n\nTry: \"/create <type>: <title>\"";
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }

        }

        [LuisIntent("")]
        public async Task noIntent(IDialogContext context, LuisResult result)
        {
            string text = "Sorry, didn't understand.  Try again";

            await context.PostAsync(text);
            context.Wait(MessageReceived);
        }

        public async Task GetOtherField(IDialogContext context, IAwaitable<Message> argument)
        {
            var incomingMessage = await argument;
            string type = null;
            string query = null;
            if (context.ConversationData.TryGetValue("type", out type))
                query = incomingMessage.Text;
            else if (context.ConversationData.TryGetValue("query", out query))
                type = incomingMessage.Text;
            //var searching = context.MakeMessage().CreateReplyMessage(, "en");
            //await connector.Messages.SendMessageAsync(searching);
            string message = $"Searching for {query} in {type}....";

            context.ConversationData.SetValue<QueryItem>("userQuery", new QueryItem() { query = query, type = type });

            await context.PostAsync(message);
            context.Wait(MessageReceived);

        }
        
        private bool TryFindType(LuisResult result, out string query, out string type)
        {
            query = null;
            type = null;

            EntityRecommendation QueryEntity, TitleEntity, TypeEntity;
            if (result.TryFindEntity(Constants.Query_Entity, out QueryEntity))
                query = QueryEntity.Entity;

            if (result.TryFindEntity(Constants.Title_Entity, out TitleEntity))
                query = TitleEntity.Entity;

            if (result.TryFindEntity(Constants.Type_Entity, out TypeEntity))
                type = TypeEntity.Entity;

            if (query != null && type != null)
                return true;

            return false;
        }
    }

}