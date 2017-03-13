using System.Threading.Tasks;
using System.Collections.Generic;
using Orleans;
using System.Linq;
using System;
using Orleans.Concurrency;

namespace Market
{   
    [Reentrant]
    public class AccountGrain : Grain<AccountGrainState>, IAccountGrain
    {
        public Task<IEnumerable<Order>> GetOpenOrders()
            => Task.FromResult(this.State.OpenOrders.AsEnumerable());

        public Task<IEnumerable<Position>> GetPositions()
            => Task.FromResult(this.State.PositionsBySymbol.Values.AsEnumerable());

        public Task SubmitOrder(Order order)
        {
            this.State.OpenOrders.Add(order);
            var securityGrain = GrainFactory.GetGrain<ISecurityGrain>(order.Symbol);
            return securityGrain.SubmitOrder(order);
        }

        public Task OrderFilled(Order order)
        {
            this.State.OpenOrders.Remove(order);

            var symbol = order.Symbol;
            var positions = this.State.PositionsBySymbol;
            if (!positions.ContainsKey(order.Symbol))
            {
                positions.Add(symbol, new Position {
                    Symbol = symbol
                });
            }
            var quantity = order.Type == OrderType.Buy ? 1 : -1;
            positions[symbol].TotalCostBasis += (order.Price * quantity);
            positions[symbol].Quantity += quantity;
            return Task.CompletedTask;
        }
    }

    public class AccountGrainState
    {
        public ISet<Order> OpenOrders { get; } = new HashSet<Order>();

        public IDictionary<string, Position> PositionsBySymbol { get; } = new Dictionary<string, Position>();
    }
}