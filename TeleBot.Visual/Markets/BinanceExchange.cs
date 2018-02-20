using Binance.Net;
using Binance.Net.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace TeleBot.Visual.Markets
{
    public class BinanceExchange : ExchangeBase
    {
        static BinanceExchange()
        {
            BinanceDefaults.SetDefaultApiCredentials(AppSettings.Default.BinanceKey, AppSettings.Default.BinanceSecret);
        }

        private static BinanceClient CreateClient()
        {
            return new BinanceClient();
        }

        public async Task<string> PlaceOrderAsync(string pair, OrderType order, decimal quantity, decimal rate)
        {
            var orderSide = order == OrderType.Buy ? OrderSide.Buy : OrderSide.Sell;
            using (var client = CreateClient())
            {
                pair = pair.Replace("-", string.Empty);
                var orderResult = await client.PlaceOrderAsync("BNBBTC", orderSide, Binance.Net.Objects.OrderType.Limit, quantity, price: rate, timeInForce: TimeInForce.GoodTillCancel);
                if (orderResult.Success)
                {
                    return orderResult.Data.ClientOrderId;
                }
                return null;
            }
        }

        public override async Task<Balance[]> GetBalancesAsync()
        {
            using (var client = CreateClient())
            {
                var accountInfo = await client.GetAccountInfoAsync();
                if (accountInfo.Success)
                {
                    var balances = accountInfo.Data.Balances;
                    var result = balances.Where(x => x.Total > 0).Select(x =>
                        new Balance
                        {
                            Total = x.Total,
                            Coin = x.Asset,
                            Locked = x.Locked,
                            Available = x.Free,
                            Exchange = "Binance",
                        }).ToArray();
                    return result;
                }
                return new Balance[0];
            }
        }

        public override async Task<TradeOrder[]> GetActiveOrdersAsync()
        {
            using (var client = CreateClient())
            {
                var openOrders = await client.GetOpenOrdersAsync();
                if (openOrders.Success)
                {
                    return openOrders.Data.Select(x =>
                        new TradeOrder
                        {
                            OrderId = x.OrderId.ToString(),
                            Price = x.Price,
                            IsCancelling = x.Status == OrderStatus.PendingCancel,
                            Side = x.Side == OrderSide.Buy ? OrderType.Buy : OrderType.Sell,
                            Symbol = x.Symbol,
                            Time = x.Time,
                            StopPrice = x.StopPrice,
                            Quantity = x.OriginalQuantity,
                            FilledQuantity = x.ExecutedQuantity,
                        }).ToArray();
                }
                return new TradeOrder[0];
            }
        }
    }
}
