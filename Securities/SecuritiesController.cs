using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Market.Securities
{
    public class SecuritiesController : Controller
    {
        [HttpGet]
        [Route("api/securities/{symbol}/quote")]
        public Quote GetQuote(string symbol)
        {
            return new Quote {
                Symbol = symbol,
                Last = 10.34m
            };
        }
    }
}
