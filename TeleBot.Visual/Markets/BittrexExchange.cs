using Bittrex.Net;
using Bittrex.Net.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeleBot.Visual.Model;

namespace TeleBot.Visual.Markets
{
    /// <summary>
    /// Bittrex emulates StopLimitLoss order.
    /// </summary>
    public class BittrexExchange : ExchangeBase
    {
        public double BtcBalance { get; protected set; }

        public Balance[] Balances { get; set; }

        static BittrexExchange()
        {
            BittrexDefaults.SetDefaultApiCredentials(AppSettings.Default.BittrexKey, AppSettings.Default.BittrexSecret);
        }

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

        public override async Task<Balance[]> GetBalancesAsync()
        {
            using (var client = CreateClient())
            {
                var result = await client.GetBalancesAsync();
                if (result.Success)
                {
                    var balances = result.Result;
                    return balances.Where(x => x.Balance > 0).Select(x =>
                        {
                            var balance = new Balance
                            {
                                Coin = x.Currency,
                                Available = x.Available ?? 0,
                                Total = x.Balance ?? 0,
                                Exchange = "Bittrex",
                            };
                            balance.Locked = balance.Total - balance.Available;
                            return balance;
                        }).ToArray();
                }
                return new Balance[0];
            }
        }

        public override async Task<TradeOrder[]> GetActiveOrdersAsync()
        {
            using (var client = CreateClient())
            {
                var result = await client.GetOpenOrdersAsync();
                if (result.Success)
                {
                    return result.Result.Select(x =>
                        new TradeOrder
                        {
                            OrderId = x.OrderUuid.ToString(),
                            Price = x.Limit,
                            IsCancelling = x.CancelInitiated,
                            Side = x.OrderSide != OrderSideExtended.LimitBuy ? OrderType.Buy : OrderType.Sell,
                            Symbol = x.Exchange,
                            Time = x.Opened,
                            Quantity = x.Quantity,
                            FilledQuantity = x.Quantity - x.QuantityRemaining,
                        }).ToArray();
                }
                return new TradeOrder[0];
            }
        }
    }
}
