using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleBot.Visual.Messenger
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _botClient;

        public event Action<TradeSignal> OnSignal;

        public event Action<string, DateTime> OnMessage;

        public TelegramBot()
        {
            _botClient = new TelegramBotClient(AppSettings.Default.TelegramBotId);
            _botClient.OnMessage += _botClient_OnMessage;
        }

        private void _botClient_OnMessage(object sender, MessageEventArgs e)
        {
            var tradeSignal = MessageParser.ProcessMessage(e.Message.Text);
            if (tradeSignal != TradeSignal.Empty)
            {
                OnSignal?.Invoke(tradeSignal);
            }
            OnMessage?.Invoke(e.Message.Text, e.Message.Date);
        }

        public void Initialize()
        {
            _botClient.StartReceiving();
        }        
    }
}
