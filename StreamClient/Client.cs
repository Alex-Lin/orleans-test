using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Streams;
using System;
using System.Threading.Tasks;
using Common;

namespace Client
{
    public class Client
    {
        private static Client instance = new Client();
        public static Client Instance => instance;
        public static IClusterClient client = null;

        public static GenericAsyncObserver<string> testObserver = new GenericAsyncObserver<string>(OnNextAsync, OnErrorAsync, OnCompleteAsync);

        public async Task InitAsync()
        {
            //configure the client with proper cluster options, logging and clustering
            ClientBuilder builder = new ClientBuilder();
            builder
              .UseLocalhostClustering()
              .Configure<ClusterOptions>(options =>
              {
                  options.ClusterId = "dev";
                  options.ServiceId = "HelloWorldApp";
              })
              .ConfigureLogging(logging => { /*logging.AddConsole();*/ });

            //  add message stream provider
            ClientStreamExtensions.AddSimpleMessageStreamProvider(builder, "TestStream");

            client = builder.Build();
            await client.Connect();

            // subscribe stream
            Guid guid = Guid.NewGuid();

            // call grains from the initialized client
            var friend = client.GetGrain<ITestStream>(0);
            var response = await friend.RegisterStream(guid);
            Console.WriteLine("\n\n{0}\n\n", response);

            var streamProvider = client.GetStreamProvider("TestStream");
            var stream = streamProvider.GetStream<string>(guid, "Test");
            await stream.SubscribeAsync(testObserver);
        }

        private static async Task OnNextAsync(string item, StreamSequenceToken token = null)
        {
            if (null != client)
            {
                Console.WriteLine($"OnNextAsync.Receive random number: {item}");
            }
            await Task.CompletedTask;
        }

        private static async Task OnErrorAsync(Exception ex)
        {
            Console.WriteLine("OnErrorAsync!!");
            await Task.CompletedTask; 
        }

        private static async Task OnCompleteAsync()
        {
            Console.WriteLine("OnCompleteAsync!!");
            await Task.CompletedTask; 
        }
    }
}
