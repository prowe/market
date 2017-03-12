using System;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Market
{
    public class OrleansHostWrapper : IDisposable
    {
        private readonly ILogger<OrleansHostWrapper> logger;
        private readonly SiloHost siloHost;

        public OrleansHostWrapper(ClusterConfiguration config, ILogger<OrleansHostWrapper> logger)
        {
            this.logger = logger;
            siloHost = new SiloHost(Dns.GetHostName(), config);
            siloHost.LoadOrleansConfig();
        }

        public void Start()
        {
            try
            {
                siloHost.InitializeOrleansSilo();
                siloHost.StartOrleansSilo(false);
            }
            catch (Exception exc)
            {
                siloHost.ReportStartupError(exc);
                throw;
            }
        }

        public void Dispose()
        {
            if (siloHost != null && siloHost.IsStarted)
            {
                try
                {
                    logger.LogInformation("Stopping Silo");
                    siloHost.StopOrleansSilo();
                    siloHost.Dispose();
                    logger.LogInformation("Silo slopped");
                }
                catch (Exception exc)
                {
                    logger.LogError(new EventId(0), exc, "Error stopping silo");
                    siloHost.ReportStartupError(exc);
                    throw;
                }
            }
        }
    }
}
