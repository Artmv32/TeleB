using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TeleBot.Visual.Messenger
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _botClient;

        public TelegramBot()
        {
            _botClient = new TelegramBotClient(AppSettings.Default.TelegramBotId);
        }

        public async Task Initialize()
        {
            _b
        }

        public async Task Run()
        {
        }
    }
}
