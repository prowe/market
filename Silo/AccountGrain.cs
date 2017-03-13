using System.Threading.Tasks;
using System.Collections.Generic;
using Orleans;
using System.Linq;
using System;

namespace Market
{
    public class AccountGrain : Grain<AccountGrainState>, IAccountGrain
    {
        public Task<IEnumerable<Order>> GetOpenOrders()
            => Task.FromResult(this.State.OpenOrders.AsEnumerable());

        public Task SubmitOrder(Order order)
        {
            this.State.OpenOrders.Add(order);
            var securityGrain = GrainFactory.GetGrain<ISecurityGrain>(order.Symbol);
            return securityGrain.SubmitOrder(order);
        }
    }

    public class AccountGrainState
    {
        public ISet<Order> OpenOrders { get; } = new HashSet<Order>();
    }
}