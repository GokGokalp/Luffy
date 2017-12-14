using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Luffy.RetryMechanism
{
    public class RetryLinearMechanismStrategy : RetryMechanismBase, IRetryMechanismStrategy
    {
        private readonly RetryMechanismOptions _retryMechanismOptions;

        public RetryLinearMechanismStrategy(RetryMechanismOptions retryMechanismOptions)
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

                await Task.Delay(_retryMechanismOptions.Interval);
            }
        }
    }
}