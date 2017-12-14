using System;
using System.Threading.Tasks;
using Luffy.CircuitBreaker;
using Luffy.RetryMechanism;

namespace Luffy
{
    public class Luffy : IExecutionOperation
    {
        private static readonly Lazy<Luffy> _Instance = new Lazy<Luffy>(() => new Luffy());
        private RetryMechanismOptions _retryMechanismOptions;
        private CircuitBreakerOptions _circuitBreakerOptions;
        private static ICircuitBreakerStateStore _circuitBreakerStateStore = new CircuitBreakerStateStore();

        public static Luffy Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        private Luffy(){}

        public Luffy UseRetry(RetryMechanismOptions retryMechanismOptions)
        {
            _retryMechanismOptions = retryMechanismOptions;

            return this;
        }

        public Luffy UseCircuitBreaker(CircuitBreakerOptions circuitBreakerOptions, ICircuitBreakerStateStore stateStore = null)
        {
            _circuitBreakerOptions = circuitBreakerOptions;

            if(stateStore != null)
            {
                _circuitBreakerStateStore = stateStore;
            }

            return this;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            if(_retryMechanismOptions == null || _circuitBreakerOptions == null)
            {
                throw new ArgumentNullException("You must use Retry or CircuitBreaker method!");
            }

            try
            {
                RetryHelper retryHelper = new RetryHelper();

                return await retryHelper.Retry(func, _retryMechanismOptions);
            }
            catch
            {
                CircuitBreakerHelper circuitBreakerHelper = new CircuitBreakerHelper(_circuitBreakerOptions, _circuitBreakerStateStore);

                return await circuitBreakerHelper.ExecuteAsync(func);
            }
        }
    }
}