using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using TeleBot.Visual.Markets;
using TeleBot.Visual.Messenger;
using TeleBot.Visual.ViewModel;

namespace TeleBot.Visual.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly TelegramBot _telegramBot = new TelegramBot();

        public ObservableCollection<TelegramMessage> NewsFeed { get; set; }

        public ObservableCollection<TradeOrder> OpenOrders { get; set; }

        public ObservableCollection<SignalVM> Signals { get; set; }


        public MainViewModel()
        {
            NewsFeed = new ObservableCollection<TelegramMessage>();
            Signals = new ObservableCollection<SignalVM>();

            _telegramBot.OnMessage += OnMessage;
            _telegramBot.OnSignal += OnSignal;
            _telegramBot.Initialize();
        }

        private void OnSignal(TradeSignal signal)
        {
            Execute.OnUIThread(() =>
            {
                Signals.Add(new SignalVM(signal));
            });
        }

        private void OnMessage(string message, DateTime date)
        {
            Execute.OnUIThread(() =>
            {
                NewsFeed.Add(new TelegramMessage { Created = date, Text = message });
            });
        }

        public void PlaceOrder(OrderVM order)
        {

        }

        public void CancelOrder(OrderVM order)
        {

        }
    }
}
