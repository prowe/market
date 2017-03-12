using System;

namespace Market
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Symbol {get; set; }

        public decimal Price { get; set; }

        public OrderType Type { get; set; }

        public override string ToString()
        {
            return $"Order #{Id} {Type} {Symbol} @ {Price}";
        }
    }

    public enum OrderType
    {
        Buy,
        Sell
    }
}