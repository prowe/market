using System.Threading.Tasks;
using Orleans;

namespace Market
{
    public class SecurityGrain : Grain, ISecurityGrain
    {
        public Task<Quote> GetQuoteAsync()
        {
            var quote = new Quote
            {
                Symbol = this.GetPrimaryKeyString()
            };
            return Task.FromResult(quote);
        }
    }
}