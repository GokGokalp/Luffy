using System.Threading.Tasks;

namespace Luffy.RetryMechanism
{
    public class RetryLinearMechanismStrategy : RetryMechanismBase, IRetryMechanismStrategy
    {
        private readonly RetryMechanismOptions _retryMechanismOptions;

        public RetryLinearMechanismStrategy(RetryMechanismOptions retryMechanismOptions)
            :base(retryMechanismOptions)
        {
            _retryMechanismOptions = retryMechanismOptions;
        }

        protected override async Task HandleBackOff(int currentRetryCount)
        {
            await Task.Delay(_retryMechanismOptions.Interval);
        }
    }
}