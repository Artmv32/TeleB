using Caliburn.Micro;
using System.Collections.ObjectModel;
using TeleBot.Visual.Markets;

namespace TeleBot.Visual.Model
{
    public class MainVM : PropertyChangedBase
    {
        public ObservableCollection<OrderVM> ActiveOrders { get; set; }

        public ObservableCollection<TelegramMessage> NewsFeed { get; set; }

        public ObservableCollection<OrderVM> SuggestedOrders { get; set; }

        public ObservableCollection<OrderVM> CompletedOrders { get; set; }

        public double CurrentBtcPrice { get; set; }

        public ObservableCollection<Balance> Balances { get; set; }

        public MainVM()
        {
            Balances = new ObservableCollection<Balance>();
        }
        
        public void PlaceOrder(OrderVM order)
        {

        }

        public void CancelOrder(OrderVM order)
        {

        }
    }
}
