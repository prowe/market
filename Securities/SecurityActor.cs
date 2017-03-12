using System.Threading.Tasks;
using Orleans;

namespace Market.Securities
{
    public class SecurityActor : Grain, ISecurityActor
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