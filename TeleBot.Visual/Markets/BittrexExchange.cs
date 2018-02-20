using Bittrex.Net;
using Bittrex.Net.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeleBot.Visual.Model;

namespace TeleBot.Visual.Markets
{
    public class BittrexExchange : ExchangeBase
    {
        public double BtcBalance { get; protected set; }

        public Balance[] Balances { get; set; }

        private static BittrexClient CreateClient()
        {
            return new BittrexClient();
        }

        public async Task<string> PlaceOrderAsync(string pair, OrderType order, decimal quantity, decimal rate)
        {
            using (var client = CreateClient())
            {
                OrderSide orderSide = order == OrderType.Buy ? OrderSide.Buy : OrderSide.Sell;
                var placedOrder = await client.PlaceOrderAsync(orderSide, pair, quantity, rate);
                if (placedOrder.Success)
                {
                    return placedOrder.Result.ToString();
                }
                return null;
            }
        }

        public async Task<OrderVM> GetOrderStatusAsync(string orderId)
        {
            using (var client = CreateClient())
            {
                var result = await client.GetOrderAsync(new Guid(orderId));
                if (result.Success)
                {
                    var order = result.Result;
                    return new OrderVM();
                }
                return null;
            }
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            using (var client = CreateClient())
            {
                var order = await client.CancelOrderAsync(new Guid(orderId));
                if (order.Success)
                {
                    return true;
                }
                return false;
            }
        }

        public override async Task<Balance[]> GetBalances()
        {
            using (var client = CreateClient())
            {
                var result = await client.GetBalancesAsync();
                if (result.Success)
                {
                    var balances = result.Result;
                    return balances.Select(x =>
                        new Balance
                        {
                            Coin = x.Currency,
                            Available = x.Available ?? 0,
                            Locked = x.Pending ?? 0,
                            Total = x.Balance ?? 0,
                            Exchange = "Bittrex",
                        }).ToArray();
                }
                return new Balance[0];
            }
        }
    }
}
