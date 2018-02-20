using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleBot.Trading;

namespace TeleBot.Tests.Trading
{
    [TestFixture]
    public class AgentTests
    {
        private readonly Mock<MarketBase> _market;
        private readonly Agent_ _target;

        private class Agent_ : Agent
        {
            public Agent_(MarketBase market) : base(market)
            {
            }

            public new void ProcessSignal(TradeSignal signal)
            {
                base.ProcessSignal(signal);
            }
        }

        public AgentTests()
        {
            _market = new Mock<MarketBase>();
            _target = new Agent_(_market.Object);
        }

        [TestCase]
        public void ProcessSignal_CurrentPriceEqualsBuyPrice_Test()
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = 10,
                BuyPriceMin = 10,
                Currency = "BTC",
                SellPrice = new[] { 20.0 },
                Term = Term.Short,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(10.0);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(10.0, (1000 * 0.05) / 10));
        }

        [TestCase]
        public void ProcessSignal_CurrentPriceEqualsBuyPriceAndStopLoss_Test()
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = 10,
                BuyPriceMin = 10,
                Currency = "BTC",
                SellPrice = new[] { 20.0 },
                Term = Term.Short,
                StopLoss = 5,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(10.0);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(10.0, (1000 * 0.05) / 10));
        }

        [TestCase(1000.0 - 1000.0 * 0.11)]
        [TestCase(1000.0 - 1000.0 * 0.5)]
        public void ProcessSignal_CurrentPriceIsMuchLower_MustBuyByLowestPriceAndOnly3PercentTest(double currentPrice)
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = 1000,
                BuyPriceMin = 1000,
                Currency = "BTC",
                SellPrice = new[] { 2000.0 },
                Term = Term.Short,
                StopLoss = 5,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(currentPrice);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(8, 1000 * 0.05));
        }

        [TestCase(1000.0 - 1000.0 * 0.01, 1000, 2000)]
        [TestCase(1000.0 - 1000.0 * 0.001, 1000, 2000)]
        [TestCase(1000.0 - 1000.0 * 0.05, 1000, 2000)]
        [TestCase(1000.0 - 1000.0 * 0.1, 1000, 2000)]
        [TestCase(1000.0 - 1000.0 * 0.101, 1000, 2000)]
        public void ProcessSignal_CurrentPriceIsALittleLower_MustBuyByLowestPriceAndOnly5PercentTest(double currentPrice, double minPrice, double maxPrice)
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = maxPrice,
                BuyPriceMin = minPrice,
                Currency = "BTC",
                SellPrice = new[] { maxPrice * 2 },
                Term = Term.Short,
                StopLoss = 5,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(currentPrice);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(8, 1000 * 0.05));
        }

        [TestCase(1000.0, 1000, 2000)]
        [TestCase(1020.0, 1000, 2000)]
        [TestCase(1500.0, 1000, 2000)]
        [TestCase(2000.0, 1000, 2000)]
        public void ProcessSignal_CurrentPriceIsInRange_MustBuy(double currentPrice, double minPrice, double maxPrice)
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = maxPrice,
                BuyPriceMin = minPrice,
                Currency = "BTC",
                SellPrice = new[] { maxPrice * 2 },
                Term = Term.Short,
                StopLoss = 5,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(currentPrice);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(8, 1000 * 0.05));
        }

        [TestCase(101.0, 200.0, 90.0, 100.0)]
        [TestCase(111.0, 200.0, 90.0, 100.0)]
        [TestCase(150.0, 200.0, 90.0, 100.0)]
        [TestCase(170.0, 200.0, 90.0, 100.0)]
        public void ProcessSignal_PriceIsHigherButRiskRewardIsAtLeast15Percent_MustBuy3Percent(double currentPrice, double sellPrice, double minPrice, double maxPrice)
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = maxPrice,
                BuyPriceMin = minPrice,
                Currency = "BTC",
                SellPrice = new[] { sellPrice },
                Term = Term.Short,
                StopLoss = 5,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(currentPrice);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(8, 1000 * 0.05));
        }

        [TestCase(175.0, 200.0, 90.0, 100.0)]
        [TestCase(190.0, 200.0, 90.0, 100.0)]
        [TestCase(200.0, 200.0, 90.0, 100.0)]
        [TestCase(210.0, 200.0, 90.0, 100.0)]
        public void ProcessSignal_PriceIsHigherButRiskRewardIsLessThan15Percent_ShouldNotBuy(double currentPrice, double sellPrice, double minPrice, double maxPrice)
        {
            // Arrange
            var signal = new TradeSignal
            {
                BuyPriceMax = maxPrice,
                BuyPriceMin = minPrice,
                Currency = "BTC",
                SellPrice = new[] { sellPrice },
                Term = Term.Short,
                StopLoss = 5,
            };
            _market.Setup(l => l.TradeCapitalSize).Returns(1000.0);
            _market.Setup(l => l.GetCurrentPrice("BTC")).Returns(currentPrice);

            // Act
            _target.ProcessSignal(signal);

            // Assert
            _market.Verify(l => l.PlaceOrder(8, 1000 * 0.05), Times.Never);
        }
    }
}
