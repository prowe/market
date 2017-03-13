
namespace Market
{
    public class Position
    {
        public string Symbol { get; set; }

        public int Quantity { get; set; } = 0;

        public decimal TotalCostBasis { get; set; } = 0m;

        public decimal AvgCostBasis
        {
            get
            {
                return TotalCostBasis / Quantity;
            }
        }
    }
}