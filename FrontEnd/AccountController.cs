using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Market
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IGrainFactory grainFactory;
        private readonly ILogger<AccountController> logger;

        public AccountController(IGrainFactory grainFactory, ILogger<AccountController> logger)
        {
            this.grainFactory = grainFactory;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/account")]
        public IPrincipal GetAccount()
        {
            return this.User;
        }

        [HttpPost]
        [Route("api/orders")]
        public async Task<Order> SubmitOrder([FromBody] Order order)
        {
            order.AccountId = this.User.Identity.Name;
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

        [Route("api/positions")]
        [HttpGet]
        public async Task<IEnumerable<Position>> GetPostitions()
        {
            return await AccountGrain.GetPositions();
        }

        private IAccountGrain AccountGrain 
            => grainFactory.GetGrain<IAccountGrain>(this.User.Identity.Name);
    }
}
