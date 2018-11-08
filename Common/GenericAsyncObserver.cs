using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GenericAsyncObserver<T> : IAsyncObserver<T>
    {
        private Func<T, StreamSequenceToken, Task> onNextAsync;
        private Func<Exception, Task> onErrorAsync;
        private Func<Task> onCompletedAsync;

        public GenericAsyncObserver(Func<T, StreamSequenceToken, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (onNextAsync == null) throw new ArgumentNullException("onNextAsync");
            if (onErrorAsync == null) throw new ArgumentNullException("onErrorAsync");
            if (onCompletedAsync == null) throw new ArgumentNullException("onCompletedAsync");

            this.onNextAsync = onNextAsync;
            this.onErrorAsync = onErrorAsync;
            this.onCompletedAsync = onCompletedAsync;
        }

        public Task OnNextAsync(T item, StreamSequenceToken token = null)
        {
            return onNextAsync(item, token);
        }

        public Task OnCompletedAsync()
        {
            return onCompletedAsync();
        }

        public Task OnErrorAsync(Exception ex)
        {
            return onErrorAsync(ex);
        }
    }
}
