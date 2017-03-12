using System;
using System.Net;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Market
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();
            config.Defaults.TraceFilePattern = null;

            var siloHost = new SiloHost(Dns.GetHostName(), config);
            siloHost.LoadOrleansConfig();

            siloHost.InitializeOrleansSilo();
            siloHost.StartOrleansSilo(false);

            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();

            siloHost.StopOrleansSilo();
            siloHost.Dispose();
        }
    }
}