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
            await AccountGrain.SubmitOrder(order);
            logger.LogInformation("Order submitted: {0}", order);
            return order;
        }

        [Route("api/orders")]
        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await AccountGrain.GetOpenOrders();
        }

        private IAccountGrain AccountGrain 
            => grainFactory.GetGrain<IAccountGrain>("abc@example.com");
    }
}
