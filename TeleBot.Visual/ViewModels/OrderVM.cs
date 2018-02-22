using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBot.Visual.ViewModel
{
    public enum OrderState
    {
        Suggested,
        Placing,
        Bought,
        Selling,
        Sold,
        Cancelling,
        Cancelled,
    }

    public class OrderVM : PropertyChangedBase
    {
        private double currentPrice;
        public double buyPrice;

        public string OrderId { get; set; }

        public DateTime Placed { get; set; }

        public string Exchange { get; set; }

        public double CurrentPrice
        {
            get { return currentPrice;  }
            set
            {
                if (currentPrice != value)
                {
                    currentPrice = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange(() => GainLoss);
                }
            }
        }

        public string Coin { get; set; }

        public double StopLoss { get; set; }

        public double BuyPrice
        {
            get { return buyPrice; }
            set
            {
                if (buyPrice != value)
                {
                    buyPrice = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange(() => GainLoss);
                }
            }
        }

        public OrderState State { get; set; }

        public double PotentialGain { get; set; }

        public double PotentialLoose { get; set; }


        public double GainLoss
        {
            get
            {
                if (double.IsNaN(currentPrice))
                    return 0;
                return CurrentPrice - BuyPrice;
            }
        }

        public ObservableCollection<OrderVM> SellOrders { get; set; }

        public OrderVM()
        {
            StopLoss = double.NaN;
            currentPrice = double.NaN;
            SellOrders = new ObservableCollection<OrderVM>();
        }
    }
}
