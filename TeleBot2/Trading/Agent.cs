using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeleBot.Trading
{
    public class MarketOrder
    {
        public string Id { get; set; }
    }

    public abstract class MarketBase
    {
        /// <summary>
        /// Money in BTC
        /// </summary>
        public virtual double TradeCapitalSize { get; set; }

        public abstract double GetCurrentPrice(string currency);

        public abstract MarketOrder PlaceOrder(double price, double amounts);

        public abstract bool IsSupported(string coin);

        public virtual void Initialize()
        {

        }
    }

    public class Agent
    {
        private Task _task;
        private Task _ordersTask;
        private readonly BlockingCollection<TradeSignal> _newSignals = new BlockingCollection<TradeSignal>(100);
        private readonly List<TradeSignal> _waiting = new List<TradeSignal>();
        private readonly BlockingCollection<MarketOrder> _orders = new BlockingCollection<MarketOrder>();
        private const double MaxRisk = 1.0 / 2.0;
        private readonly MarketBase _market;

        public Agent(MarketBase market)
        {
            if (market == null)
            {
                throw new ArgumentNullException("market");
            }
            _market = market;
        }

        public void Start()
        {
            _task = Task.Run(() =>
            {
                Initialize();
                _ordersTask = Task.Run(new Action(ProcessOrders));
                ExecutionLoop();
            });
        }

        private void Initialize()
        {
            _market.Initialize();
        }

        public void Enqueue(TradeSignal signal)
        {
            if (signal == null)
            {
                throw new ArgumentNullException("signal");
            }

            if (!_market.IsSupported(signal.Currency))
            {
                throw new InvalidOperationException($"Currency {signal.Currency} is not supported.");
            }

            _newSignals.Add(signal);
        }

        private void ExecutionLoop()
        {
            while (!_newSignals.IsCompleted)
            {
                try
                {
                    var signal = _newSignals.Take();
                    var order = ProcessSignal(signal);
                    if (order != null)
                    {
                        _orders.Add(order);
                    }
                }
                catch (InvalidOperationException)
                {

                }
            }
        }

        protected MarketOrder ProcessSignal(TradeSignal signal)
        {
            if (signal == null)
            {
                throw new ArgumentNullException("signal");
            }

            var analysis = Analyzer.Analyze(signal);
            var currentPrice = _market.GetCurrentPrice(signal.Currency);
            // 1. currentPrice < MinBuyPrice
            // 2. currentPrice = [MinBuyPrice, MaxBuyPrice]
            // 3. currentPrice > MaxBuyPrice
            // RiskReward at least 1:2

            var moneyAtRisk = _market.TradeCapitalSize * 0.05;
            var coins = Math.Floor(moneyAtRisk / currentPrice);
            if (currentPrice >= signal.BuyPriceMin && currentPrice <= signal.BuyPriceMax)
            {
                var order = _market.PlaceOrder(currentPrice, coins);
                return order;
            }
            return null;
        }

        private void ProcessOrders()
        {
            while (!_newSignals.IsCompleted)
            {
                try
                {
                    var order = _orders.Take();
                    
                }
                catch (InvalidOperationException)
                {

                }
            }
        }
    }
}
