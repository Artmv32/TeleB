using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;

namespace TeleBot.Visual.ViewModels
{
    public class TradeViewModel : PropertyChangedBase
    {
        public decimal Invested { get; set; }

        public ObservableCollection<TradeOrderViewModel> Orders { get; set; }

        public TradeViewModel()
        {
            Orders = new ObservableCollection<TradeOrderViewModel>();
        }
    }
}
