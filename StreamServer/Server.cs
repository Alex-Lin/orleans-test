using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Net;

namespace Server
{
    public class Server
    {
        private static Server instance = new Server();
        public static Server Instance => instance;

        public async Task InitAsync()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .ConfigureLogging(logging => { /*logging.AddConsole();*/ });

            builder.AddSimpleMessageStreamProvider("TestStream")
           .AddMemoryGrainStorage("PubSubStore");

            var host = builder.Build();
            await host.StartAsync();
        } 
    }
}
