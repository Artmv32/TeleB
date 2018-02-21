using Caliburn.Micro;
using System.Linq;

namespace TeleBot.Visual.ViewModel
{
    public sealed class SignalVM : PropertyChangedBase
    {
        public string Currency { get; set; }

        public decimal BuyPriceMin { get; set; }

        public decimal BuyPriceMax { get; set; }

        public decimal[] SellPrice { get; set; }

        public decimal StopLoss { get; set; }

        public Term Term { get; set; }

        public SignalVM()
        {

        }

        public SignalVM(TradeSignal signal)
        {
            if (signal != null)
            {
                Currency = signal.Currency;
                BuyPriceMin = (decimal)signal.BuyPriceMin;
                BuyPriceMax = (decimal)signal.BuyPriceMax;                
                StopLoss = (decimal) (double.IsNaN(signal.StopLoss) ? 0 : signal.StopLoss);
                
                Term = signal.Term;
                if (signal.SellPrice != null)
                {
                    SellPrice = signal.SellPrice.Select(x => (decimal)x).ToArray();
                }
            }
        }
    }
}
