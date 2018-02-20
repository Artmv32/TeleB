using Binance.Net;
using Binance.Net.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace TeleBot.Visual.Markets
{
    public class BinanceExchange : ExchangeBase
    {

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

        public override async Task<Balance[]> GetBalances()
        {
            using (var client = CreateClient())
            {
                var accountInfo = await client.GetAccountInfoAsync();
                if (accountInfo.Success)
                {
                    var balances = accountInfo.Data.Balances;
                    var result = balances.Select(x =>
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
    }
}
