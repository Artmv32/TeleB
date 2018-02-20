using System;
using System.Threading.Tasks;

namespace TeleBot
{
    class Program
    {
        const string botId = @"529680487:AAFP2d-wSsdjlr2Zw8NT52wDnMWOKnItwqQ2";
        const string fromOnly = "Artmv32";
        private static int updateId = 0;

        static void Main(string[] args)
        {
            RunBot().Wait();
            
            Console.WriteLine("Hello World!");
        }

        private static async Task RunBot()
        {
            var botClient = new Telegram.Bot.TelegramBotClient(botId);
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Hello! My name is {me.FirstName}");
            var updates = await botClient.GetUpdatesAsync(offset: updateId);
            Console.WriteLine("Found some updates. " + updates.Length);
            foreach (var update in updates)
            {
                updateId = update.Id + 1;
                if (string.Equals(update.Message.From.Username, fromOnly, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Received a message from Andrey.");
                    if (update.Message.Text == "Privet")
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Privet andrey");
                        Console.WriteLine("Message sent");
                    }
                }
            }
            
        }
    }
}
