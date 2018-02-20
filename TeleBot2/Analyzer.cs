using System;

namespace TeleBot
{
    public class Analysis
    {
        public double PossibleGains { get; set; }

        public double RiskReward { get; set; }

        public string Currency { get; set; }
        public bool HasRisk
        {
            get { return !double.IsNaN(RiskReward); }
        }

        public Analysis()
        {
            RiskReward = double.NaN;
        }
    }

    public class Analyzer
    {
        public static Analysis Analyze(TradeSignal action)
        {
            if (action == null || ReferenceEquals(action, TradeSignal.Empty))
            {
                return null;
            }
            if (!double.IsNaN(action.StopLoss) && action.StopLoss < 0)
            {
                throw new InvalidOperationException("StopLoss is less than zero.");
            }

            var buyPrice = action.BuyPriceMin;
            double sellFraction = 1.0 / action.SellPrice.Length; // Assume we bought one coin

            double earned = 0.0;
            foreach (var sellPrice in action.SellPrice)
            {
                if (sellPrice > 0.0)
                {
                    earned += sellPrice * sellFraction;
                }
                else
                {
                    throw new InvalidOperationException("SellPrice is less than zero.");
                }
            }
            double potentialGains = (earned - buyPrice) / buyPrice;
            double riskReward = double.NaN;
            if (!double.IsNaN(action.StopLoss))
            {
                riskReward = (buyPrice - action.StopLoss) / (earned - buyPrice);
            }

            var result = new Analysis
            {
                Currency = action.Currency,
                PossibleGains = potentialGains,
                RiskReward = riskReward,
            };

            return result;
        }
    }
}
