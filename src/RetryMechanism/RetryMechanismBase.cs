using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LuffyCore.RetryMechanism
{
    public abstract class RetryMechanismBase
    {
        private readonly RetryMechanismOptions _retryMechanismOptions;

        public RetryMechanismBase(RetryMechanismOptions retryMechanismOptions)
        {
            _retryMechanismOptions = retryMechanismOptions;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            int currentRetryCount = 0;

            for(;;)
            {
                try
                {
                    return await func.Invoke();
                }
                catch(Exception ex)
                {
                    currentRetryCount++;

                    bool isTransient = await IsTransient(ex);
                    if(currentRetryCount > _retryMechanismOptions.RetryCount || !isTransient)
                    {
                        throw;
                    }
                }

                await HandleBackOff(currentRetryCount);

                await Task.Delay(_retryMechanismOptions.Interval);
            }
        }

        protected abstract Task HandleBackOff(int currentRetryCount);
        
        private Task<bool> IsTransient(Exception ex)
        {
            bool isTransient = false;
            var webException = ex as WebException;

            if(webException != null)
            {
                isTransient = new[] {WebExceptionStatus.ConnectionClosed,
                              WebExceptionStatus.Timeout,
                              WebExceptionStatus.RequestCanceled,
                              WebExceptionStatus.KeepAliveFailure,
                              WebExceptionStatus.PipelineFailure,
                              WebExceptionStatus.ReceiveFailure,
                              WebExceptionStatus.ConnectFailure,
                              WebExceptionStatus.SendFailure}
                              .Contains(webException.Status);

                return Task.FromResult(isTransient);
            }

            return Task.FromResult(isTransient);
        }
    }
}