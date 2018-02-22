using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeleBot.Visual.Markets
{
    public class Balance
    {
        public string Coin { get; set; }

        public decimal Total { get; set; }

        public decimal Available { get; set; }

        public decimal Locked { get; set; }

        public string Exchange { get; internal set; }
    }

    public class TradeOrder
    {
        public string OrderId { get; internal set; }
        public decimal Price { get; internal set; }
        public OrderType Side { get; internal set; }
        public DateTime Time { get; internal set; }
        public decimal Quantity { get; internal set; }
        public decimal FilledQuantity { get; internal set; }
        public string Symbol { get; internal set; }
        public decimal StopPrice { get; internal set; }
        public bool IsCancelling { get; internal set; }
    }

    public enum OrderType
    {
        Buy,
        Sell,
    }

    public abstract class ExchangeBase
    {
        public abstract bool SupportsLimitStopLoss { get; }

        public abstract Task<Balance[]> GetBalancesAsync();

        public abstract Task<TradeOrder[]> GetActiveOrdersAsync();

        public virtual Task<string> PlaceLimitStopLossOrderAsync(string pair, OrderType order, decimal quantity, decimal rate, decimal stopLoss)
        {
            throw new NotImplementedException();
        }

        public abstract Task<string> PlaceOrderAsync(string pair, OrderType order, decimal quantity, decimal rate);
    }
}
