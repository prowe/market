using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using System;

namespace Market
{
    public interface IAccountGrain : IGrainWithStringKey
    {
        Task<IEnumerable<Order>> GetOpenOrders();

        Task<IEnumerable<Position>> GetPositions();

        Task SubmitOrder(Order order);

        Task OrderFilled(Order order);

        Task CancelOrder(Guid orderId);
    }
}