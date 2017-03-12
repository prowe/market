using System.Threading.Tasks;
using Orleans;

namespace Market.Securities
{
    public interface ISecurityActor : IGrainWithStringKey
    {
        Task<Quote> GetQuoteAsync();
    }
}