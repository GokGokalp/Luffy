using System;
using System.Threading.Tasks;

namespace LuffyCore.RetryMechanism
{
    public class RetryHelper
    {
        public async Task<T> Retry<T>(Func<Task<T>> func, RetryMechanismOptions retryMechanismOptions)
        {
            IRetryMechanismStrategy retryMechanism = null;

            if(retryMechanismOptions.RetryPolicies == RetryPolicies.Linear)
            {
                retryMechanism = new RetryLinearMechanismStrategy(retryMechanismOptions);
            }

            return await retryMechanism.ExecuteAsync(func);
        }
    }
}