using Caliburn.Micro;
using System;
using TeleBot.Visual.Markets;

namespace TeleBot.Visual.ViewModels
{
    public sealed class TradeOrderViewModel : PropertyChangedBase
    {
        private TradeOrder _order;

        public string OrderId
        {
            get { return _order.OrderId; }
        }

        public decimal Price
        {
            get { return _order.Price; }
        }

        public Markets.OrderType Side
        {
            get { return _order.Side; }
        }
        
        public DateTime Time
        {
            get { return _order.Time; }
        }

        public decimal Quantity
        {
            get { return _order.Quantity; }
        }

        public decimal FilledQuantity
        {
            get { return _order.FilledQuantity; }
        }

        public string Symbol
        {
            get { return _order.Symbol; }
        }

        public decimal StopPrice
        {
            get { return _order.StopPrice; }
        }

        public bool IsCancelling
        {
            get { return _order.IsCancelling; }
        }

        public TradeOrderViewModel(TradeOrder order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            _order = order;
        }

        public void SetTradeOrder(TradeOrder order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            _order = order;
            NotifyOfPropertyChange();
        }
    }
}
