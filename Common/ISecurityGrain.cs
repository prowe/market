using System.Threading.Tasks;
using Orleans;

namespace Market
{
    public interface ISecurityGrain : IGrainWithStringKey
    {
        Task<Quote> GetQuoteAsync();
    }
}