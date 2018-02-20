using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleBot.Trading;

namespace TeleBot2.Trading
{
    public class OrdersManager
    {
        private readonly BlockingCollection<MarketOrder> _newSignals = new BlockingCollection<MarketOrder>(100);

        public void Run()
        {

        }
    }
}
