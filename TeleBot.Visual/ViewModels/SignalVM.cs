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

        public float PotentialGain { get; set; }

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
                    UpdatePotentialGain();
                }
            }
        }

        private void UpdatePotentialGain()
        {
            if (SellPrice?.Any() == false)
            {
                return;
            }
            const decimal invested = 0.01M; // BTC
            decimal investedCoins = invested / BuyPriceMax;
            decimal sellFraction = investedCoins / SellPrice.Length;
            decimal expectedGain = 0;
            foreach (var price in SellPrice)
            {
                expectedGain += price * sellFraction;
            }

            decimal diff = expectedGain - invested;
            PotentialGain = (float)(diff / invested);
        }
    }
}
