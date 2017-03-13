using System.Threading.Tasks;
using System.Collections.Generic;
using Orleans;
using System.Linq;
using System;

namespace Market
{
    public class SecurityGrain : Grain<SecurityGrainState>, ISecurityGrain
    {
        public Task<Quote> GetQuote()
        {
            var quote = new Quote
            {
                Symbol = this.GetPrimaryKeyString(),
                Bid = this.State.Bids.FirstOrDefault()?.Price,
                Ask = this.State.Offers.FirstOrDefault()?.Price
            };
            return Task.FromResult(quote);
        }

        public Task SubmitOrder(Order order)
        {
            if (order.Type == OrderType.Buy)
            {
                this.State.Bids.Add(order);
            }
            if (order.Type == OrderType.Sell)
            {
                this.State.Offers.Add(order);
            }
            return Task.CompletedTask;
        }
    }

    public class SecurityGrainState
    {
        public SortedSet<Order> Bids { get; } = new SortedSet<Order>(new OrderPriceDescComparator());

        public SortedSet<Order> Offers {get; } = new SortedSet<Order>(new OrderPriceAscComparator());
    }

    public class OrderPriceAscComparator : Comparer<Order>
    {
        public override int Compare(Order x, Order y)
        {
            return decimal.Compare(x.Price, y.Price);
        }
    }

        public class OrderPriceDescComparator : Comparer<Order>
    {
        public override int Compare(Order x, Order y)
        {
            return decimal.Compare(y.Price, x.Price);
        }
    }
}