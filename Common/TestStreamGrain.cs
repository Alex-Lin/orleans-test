using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public interface ITestStream : IGrainWithIntegerKey
    {
        Task<string> RegisterStream(Guid guid);
    }

    public class TestStreamGrain : Grain, ITestStream
    {
        Task<string> ITestStream.RegisterStream(Guid guid)
        {
            //Get one of the providers which we defined in config
            var streamProvider = GetStreamProvider("TestStream");

            //Get the reference to a stream
            var stream = streamProvider.GetStream<string>(guid, "Test");

            RegisterTimer(_ =>
            {
                return stream.OnNextAsync(new System.Random().Next().ToString());
            }, null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(1000));

            return Task.FromResult($"RegisterStream guid: '{guid}'");
        } 

        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            return base.OnDeactivateAsync();
        }
    }
}
