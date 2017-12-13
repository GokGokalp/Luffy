using System;
using System.Threading;

namespace Luffy.CircuitBreaker
{
    public class CircuitBreaker
    {
        private readonly ICircuitBreakerOptions _circuitBreakerOptions;
        private readonly ICircuitBreakerStateStore _stateStore;
        private readonly object _halfOpenSyncObject = new Object();
        public bool IsClosed { get { return _stateStore.IsClosed; } }
        public bool IsOpen { get { return !IsClosed; } }

        public CircuitBreaker(ICircuitBreakerOptions circuitBreakerOptions, ICircuitBreakerStateStore stateStore)
        {
            _circuitBreakerOptions = circuitBreakerOptions;
            _stateStore = stateStore;
        }

        public void ExecuteAction(Action action)
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
                            _stateStore.HalfOpen();

                            action();

                            _stateStore.Reset();
                            return;
                        }
                    }
                    catch(Exception ex)
                    {
                        _stateStore.Trip(ex);
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
                action();
            }
            catch(Exception ex)
            {
                _stateStore.Trip(ex);
                throw;
            }
        }
    }
}