using NUnit.Framework;
using System;

namespace TeleBot.Tests
{
    [TestFixture]
    public class AnalyzerTests
    {
        private Analyzer Analyzer = new Analyzer();

        [TestCase]
        public void Analyze_SingleSellPrice()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 100,
                BuyPriceMin = 100,
                Currency = "ASDA",
                SellPrice = new[] { 115.0 },
                Term = Term.Short,
            };

            var result = Analyzer.Analyze(action);

            Assert.AreEqual(0.15, result.PossibleGains);
            Assert.AreEqual(double.NaN, result.RiskReward);
            Assert.AreEqual("ASDA", result.Currency);
        }

        [TestCase]
        public void Analyze_SingleSellPriceAndStopLoss()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 100,
                BuyPriceMin = 100,
                Currency = "ASDA",
                SellPrice = new[] { 140.0 },
                StopLoss = 80,
                Term = Term.Short,
            };

            var result = Analyzer.Analyze(action);

            Assert.AreEqual(0.4, result.PossibleGains);
            Assert.AreEqual(20.0 / 40.0, result.RiskReward);
            Assert.AreEqual("ASDA", result.Currency);
        }

        [TestCase]
        public void Analyze_ManySellPrices()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 100,
                BuyPriceMin = 100,
                Currency = "BTC",
                SellPrice = new[] { 110.0, 120.0, 130.0, 140.0 },
                Term = Term.Short,
            };

            var result = Analyzer.Analyze(action);

            Assert.AreEqual(0.25, result.PossibleGains);
            Assert.AreEqual(double.NaN, result.RiskReward);
            Assert.AreEqual("BTC", result.Currency);
        }

        [TestCase]
        public void Analyze_BigGain()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 0.00005,
                BuyPriceMin = 0.00005,
                Currency = "TRST",
                SellPrice = new[] { 0.000575 },
                Term = Term.Short,
            };

            var result = Analyzer.Analyze(action);

            Assert.AreEqual(10.5, result.PossibleGains, 10.5 - 10.49);
            Assert.AreEqual(double.NaN, result.RiskReward);
            Assert.AreEqual("TRST", result.Currency);
        }

        [TestCase]
        public void Analyze_NegativeSellPrice()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 0.00005,
                BuyPriceMin = 0.00005,
                Currency = "TRST",
                SellPrice = new[] { 1, -0.000575, 3 },
                Term = Term.Short,
            };

            Assert.Throws<InvalidOperationException>(() => Analyzer.Analyze(action));
        }

        [TestCase]
        public void Analyze_NegativeStoLoss()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 0.00005,
                BuyPriceMin = 0.00005,
                Currency = "TRST",
                SellPrice = new[] { 1, 0.000575, 3 },
                StopLoss = -2,
                Term = Term.Short,
            };

            Assert.Throws<InvalidOperationException>(() => Analyzer.Analyze(action));
        }

        [TestCase]
        public void Analyze_RangeOfBuyPrices()
        {
            var action = new TradeSignal
            {
                BuyPriceMax = 150,
                BuyPriceMin = 100,
                Currency = "TRST",
                SellPrice = new[] { 200.0 },
                Term = Term.Short,
            };

            var result = Analyzer.Analyze(action);

            Assert.AreEqual(1, result.PossibleGains);
            Assert.AreEqual(double.NaN, result.RiskReward);
            Assert.AreEqual("TRST", result.Currency);
        }
    }
}
