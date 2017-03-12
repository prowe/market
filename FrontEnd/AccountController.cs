using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Market
{
    public class AccountController : Controller
    {
        private readonly IGrainFactory grainFactory;
        private readonly ILogger<AccountController> logger;

        public AccountController(IGrainFactory grainFactory, ILogger<AccountController> logger)
        {
            this.grainFactory = grainFactory;
            this.logger = logger;
        }

        [HttpPost]
        [Route("api/orders")]
        public async Task<Order> SubmitOrder([FromBody] Order order)
        {
            logger.LogInformation("Submitting order: {0}", order);
            var securityGrain = grainFactory.GetGrain<ISecurityGrain>(order.Symbol);
            await securityGrain.SubmitOrderAsync(order);
            logger.LogInformation("Order submitted: {0}", order);
            return order;
        }
    }
}
