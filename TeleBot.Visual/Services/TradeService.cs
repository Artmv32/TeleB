using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeleBot.Visual.Markets;
using TeleBot.Visual.ViewModel;
using TeleBot.Visual.ViewModels;

namespace TeleBot.Visual.Services
{
    public class TradeTask
    {
        public decimal Amount { get; set; }

        public decimal Price { get; set; }
    }

    public class OrdersTask
    {
        public TradeTask BuyTask { get; set; }

        public decimal StopLoss { get; set; }

        public string Currency { get; set; }

        public IList<TradeTask> SellTasks { get; set; }
    }

    public sealed class TradeService
    {
        public OrdersTask CreateTradeFromSignal(SignalVM signal, decimal investCapital, decimal currentPrice)
        {
            if (signal == null)
            {
                throw new ArgumentNullException("signal");
            }
            if (signal.SellPrice?.Any() == false)
            {
                throw new InvalidOperationException();
            }

            var sellTasks = new List<TradeTask>(signal.SellPrice.Length);
            decimal amount = currentPrice / investCapital;
            var buyTask = new TradeTask
            {
                Price = currentPrice,
                Amount = amount,
            };

            decimal sellPortion = amount / signal.SellPrice.Length;
            foreach (var sellPrice in signal.SellPrice)
            {
                sellTasks.Add(new TradeTask
                {
                    Price = sellPrice,
                    Amount = sellPortion,

                });
            }

            var result = new OrdersTask
            {
                Currency = signal.Currency,
                StopLoss = signal.StopLoss,
                BuyTask = buyTask,
                SellTasks = sellTasks,
            };

            return result;
        }

        public async Task ProcessTask(OrdersTask task, ExchangeBase exchange)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            if (exchange == null)
            {
                throw new ArgumentNullException("exchange");
            }

            string buyOrder = await exchange.PlaceOrderAsync(task.Currency, OrderType.Buy, task.BuyTask.Amount, task.BuyTask.Price);
            var sellOrders = new List<string>(task.SellTasks.Count);
            foreach (var sellTask in task.SellTasks)
            {
                var sellOrder = await exchange.PlaceLimitStopLossOrderAsync(task.Currency, OrderType.Sell, sellTask.Amount, sellTask.Price, task.StopLoss);
                sellOrders.Add(sellOrder);
            }
        }
    }
}
