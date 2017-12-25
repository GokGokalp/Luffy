using System;
using System.Threading.Tasks;
using LuffyCore.CircuitBreaker;
using LuffyCore.RetryMechanism;

namespace LuffyCore
{
    public class Luffy : IExecutionOperation
    {
        private RetryMechanismOptions _retryMechanismOptions;
        private CircuitBreakerOptions _circuitBreakerOptions;
        private static ICircuitBreakerStateStore _circuitBreakerStateStore = new CircuitBreakerStateStore();

        public static Luffy Instance
        {
            get
            {
                return new Luffy();
            }
        }

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
            if(_retryMechanismOptions == null && _circuitBreakerOptions == null)
            {
                throw new ArgumentNullException("You must use Retry or CircuitBreaker method!");
            }

            try
            {
                if(_retryMechanismOptions != null)
                {
                    RetryHelper retryHelper = new RetryHelper();

                    return await retryHelper.Retry(func, _retryMechanismOptions);
                }

                return await func.Invoke();
            }
            catch
            {
                if(_circuitBreakerOptions != null)
                {
                    CircuitBreakerHelper circuitBreakerHelper = new CircuitBreakerHelper(_circuitBreakerOptions, _circuitBreakerStateStore);

                    return await circuitBreakerHelper.ExecuteAsync(func);
                }

                throw;         
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func, Func<Task<T>> fallbackFunc)
        {
            try
            {
                return await ExecuteAsync(func);
            }
            catch
            {
                if(fallbackFunc != null)
                {
                    var result = await fallbackFunc.Invoke();

                    return result;
                }

                throw;
            }  
        }
    }
}