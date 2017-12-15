using System;
using System.Threading.Tasks;

namespace LuffyCore.RetryMechanism
{
    public class RetryExponentiallyMechanismStrategy : RetryMechanismBase, IRetryMechanismStrategy
    {
        private readonly RetryMechanismOptions _retryMechanismOptions;

        public RetryExponentiallyMechanismStrategy(RetryMechanismOptions retryMechanismOptions)
         : base(retryMechanismOptions)
        {
            _retryMechanismOptions = retryMechanismOptions;
        }

        protected override async Task HandleBackOff(int currentRetryCount)
        {
            await Task.Delay(TimeSpan.FromSeconds(Math.Pow(_retryMechanismOptions.Interval.Seconds, currentRetryCount)));
        }
    }
}