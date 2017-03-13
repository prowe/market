using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Market
{
    public interface IAccountGrain : IGrainWithStringKey
    {
        Task<IEnumerable<Order>> GetOpenOrders();

        Task<IEnumerable<Position>> GetPositions();

        Task SubmitOrder(Order order);

        Task OrderFilled(Order order);
    }
}