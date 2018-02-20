using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleBot.Trading;

namespace TeleBot.Markets
{
    public class WhaleClubMarket : MarketBase
    {
        private const string Endpoint = @"https://api.whaleclub.co/v1/";

        public override double GetCurrentPrice(string currency)
        {
            throw new NotImplementedException();
        }

        public override bool IsSupported(string coin)
        {
            throw new NotImplementedException();
        }
        
        public override MarketOrder PlaceOrder(double price, double amounts)
        {
            throw new NotImplementedException();
        }
    }
}
