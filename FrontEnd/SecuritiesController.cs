using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Market.Securities
{
    public class SecuritiesController : Controller
    {
        private readonly IGrainFactory grainFactory;
        private readonly ILogger<SecuritiesController> logger;

        public SecuritiesController(IGrainFactory grainFactory, ILogger<SecuritiesController> logger)
        {
            this.grainFactory = grainFactory;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/securities/{symbol}/quote")]
        public async Task<Quote> GetQuote(string symbol)
        {
            var securityGrain = grainFactory.GetGrain<ISecurityGrain>(symbol);
            return await securityGrain.GetQuoteAsync();
        }
    }
}
