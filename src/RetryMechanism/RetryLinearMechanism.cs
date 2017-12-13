using System;
using System.Threading;
using System.Threading.Tasks;

namespace Luffy.RetryMechanism
{
    public class RetryLinearMechanism : IRetryMechanism
    {
        private readonly RetryMechanismOptions _retryMechanismOptions;

        public RetryLinearMechanism(RetryMechanismOptions retryMechanismOptions)
        {
            _retryMechanismOptions = retryMechanismOptions;
        }

        public Task Execute(Action action)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(Func<Task> func)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            throw new NotImplementedException();
        }
    }
}