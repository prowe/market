using System;

namespace Market
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Symbol { get; set; }

        public decimal Price { get; set; }

        public OrderType Type { get; set; }

        public override string ToString()
            => $"Order #{Id} {Type} {Symbol} @ {Price}";

        public override bool Equals(object obj)
            => Id.Equals((obj as Order)?.Id);

        public override int GetHashCode() 
            => Id.GetHashCode();
    }

    public enum OrderType
    {
        Buy,
        Sell
    }
}