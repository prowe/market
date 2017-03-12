namespace Market
{
    public class Quote
    {
        public string Symbol { get; set; }

        public decimal? Last { get; set; }

        public decimal? Bid { get; set; }

        public decimal? Ask { get; set; }
    }
}