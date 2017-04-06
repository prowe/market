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

        public async Task SubmitOrder(Order order)
        {
            PushOrderToLevel2(order);
            await CheckForFill();
            await this.WriteStateAsync();
        }

        public async Task CancelOrder(Order order)
        {
            State.Bids.Remove(order);
            State.Offers.Remove(order);
            await this.WriteStateAsync(); 
        }

        private void PushOrderToLevel2(Order order)
        {
            var stack = order.Type == OrderType.Buy
                ? this.State.Bids
                : this.State.Offers;
            stack.Add(order);
        }

        private Task CheckForFill()
        {
            var bids = this.State.Bids;
            if (!bids.Any())
            {
                return Task.CompletedTask;
            }
            var offers = this.State.Offers;
            if (!offers.Any())
            {
                return Task.CompletedTask;
            }

            var headBid = bids.FirstOrDefault();
            var headOffer = offers.FirstOrDefault();
            if(headBid.Price >= headOffer.Price)
            {
                GetLogger().TrackTrace("Filling orders: " + headBid + headOffer);
                return Task.WhenAll(
                    ExecuteFill(headBid, bids),
                    ExecuteFill(headOffer, offers)
                );
            }

            return Task.CompletedTask;
        }

        private Task ExecuteFill(Order filledOrder, ISet<Order> orderQueue)
        {
            orderQueue.Remove(filledOrder);
            var account = GrainFactory.GetGrain<IAccountGrain>(filledOrder.AccountId);
            return account.OrderFilled(filledOrder);
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