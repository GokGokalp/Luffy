using System;
using System.Threading;
using System.Threading.Tasks;

namespace LuffyCore.CircuitBreaker
{
    public class CircuitBreakerHelper : IExecutionOperation
    {
        private readonly CircuitBreakerOptions _circuitBreakerOptions;
        private readonly ICircuitBreakerStateStore _stateStore;
        private readonly object _halfOpenSyncObject = new Object();
        private bool IsClosed { get { return _stateStore.IsClosed; } }
        private bool IsOpen { get { return !IsClosed; } }

        public CircuitBreakerHelper(CircuitBreakerOptions circuitBreakerOptions, ICircuitBreakerStateStore stateStore)
        {
            _circuitBreakerOptions = circuitBreakerOptions;
            _stateStore = stateStore;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            if(IsOpen)
            {
                if(_stateStore.LastStateChangedDateUtc.Add(_circuitBreakerOptions.DurationOfBreak) < DateTime.UtcNow)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(_halfOpenSyncObject, ref lockTaken);
                        if(lockTaken)
                        {
                            HalfOpen();

                            var result = await func.Invoke();

                            Reset();

                            return result;
                        }
                    }
                    catch(Exception ex)
                    {
                        Trip(ex);
                        throw;
                    }
                    finally
                    {
                        if(lockTaken)
                        {
                            Monitor.Exit(_halfOpenSyncObject);
                        }
                    }
                }

                throw new CircuitBreakerOpenException("Circuit breaker timeout hasn't yet expired.", _stateStore.LastException);
            }

            try
            {
                var result = await func.Invoke();

                return result;
            }
            catch(Exception ex)
            {
                Trip(ex);
                throw;
            }
        }

        private void HalfOpen()
        {
            _stateStore.State = CircuitBreakerStateEnum.HalfOpen;
            _stateStore.LastStateChangedDateUtc = DateTime.UtcNow;
        }
        private void Reset()
        {
            if(_stateStore.SuccessAttempt >= _circuitBreakerOptions.SuccessThresholdWhenCircuitBreakerHalfOpenStatus)
            {
                _stateStore.IsClosed = true;
                _stateStore.ExceptionAttempt = 0;
                _stateStore.SuccessAttempt = 0;
            }
        }

        private void Trip(Exception ex)
        {
            _stateStore.ExceptionAttempt++;

            if(_stateStore.ExceptionAttempt >= _circuitBreakerOptions.ExceptionThreshold)
            {                
                _stateStore.LastException = ex;
                _stateStore.State = CircuitBreakerStateEnum.Open;
                _stateStore.LastStateChangedDateUtc = DateTime.UtcNow;
            }
        }
    }
}