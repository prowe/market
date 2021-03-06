using System.Threading.Tasks;
using Orleans;

namespace Market
{
    public interface ISecurityGrain : IGrainWithStringKey
    {
        Task<Quote> GetQuote();
        Task SubmitOrder(Order order);
        Task CancelOrder(Order order);
    }
}