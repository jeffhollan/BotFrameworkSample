using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            Interactive(new VSTF_RD_Bot.VSTFDialog());
        }

        static void Interactive(IDialog form)
        {
            var message = new Message()
            {
                ConversationId = Guid.NewGuid().ToString(),
                Text = ""
            };
            string prompt;
            do
            {
                var task = Conversation.SendAsync(message, () => form);
                message = task.GetAwaiter().GetResult();
                prompt = message.Text;
                if (prompt != null)
                {
                    Console.WriteLine(prompt);
                    Console.Write("> ");
                    message.Text = Console.ReadLine();
                }
            } while (prompt != null);
        }

    }
}
