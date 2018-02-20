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

    public enum OrderType
    {
        Buy,
        Sell,
    }

    public abstract class ExchangeBase
    {
        public abstract Task<Balance[]> GetBalancesAsync();
    }
}
