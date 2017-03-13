using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Market
{
    public class SecurityController : Controller
    {
        private readonly IGrainFactory grainFactory;
        private readonly ILogger<SecurityController> logger;

        public SecurityController(IGrainFactory grainFactory, ILogger<SecurityController> logger)
        {
            this.grainFactory = grainFactory;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/securities/{symbol}/quote")]
        public async Task<Quote> GetQuote(string symbol)
        {
            var securityGrain = grainFactory.GetGrain<ISecurityGrain>(symbol);
            return await securityGrain.GetQuote();
        }
    }
}
