using System;
using System.Collections.Concurrent;

namespace LuffyCore.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private readonly ConcurrentDictionary<string, CircuitBreakerStateModel> _store = new ConcurrentDictionary<string, CircuitBreakerStateModel>();

        public void ChangeLastStateChangedDateUtc(string key, DateTime date)
        {
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                stateModel.LastStateChangedDateUtc = date;
                _store[key] = stateModel;
            }
        }

        public void ChangeState(string key, CircuitBreakerStateEnum state)
        {
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                stateModel.State = state;
                _store[key] = stateModel;
            }
        }

        public int GetExceptionAttempt(string key)
        {
            int exceptionAttempt = 0;
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                exceptionAttempt = stateModel.ExceptionAttempt;
            }

            return exceptionAttempt;
        }

        public void IncreaseExceptionAttemp(string key)
        {
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                stateModel.ExceptionAttempt += 1;
                _store[key] = stateModel;
            }
            else
            {
                stateModel = new CircuitBreakerStateModel();
                stateModel.ExceptionAttempt += 1;

                AddStateModel(key, stateModel);
            }
        }

        public DateTime GetLastStateChangedDateUtc(string key)
        {
            DateTime lastStateChangedDateUtc = default(DateTime);
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                lastStateChangedDateUtc = stateModel.LastStateChangedDateUtc;
            }

            return lastStateChangedDateUtc;
        }

        public int GetSuccessAttempt(string key)
        {
            int successAttempt = 0;
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                successAttempt = stateModel.SuccessAttempt;
            }

            return successAttempt;
        }

        public void IncreaseSuccessAttemp(string key)
        {
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                stateModel.SuccessAttempt += 1;
                _store[key] = stateModel;
            }
        }

        public bool IsClosed(string key)
        {
            bool isClosed = true;
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                isClosed = stateModel.IsClosed;
            }

            return isClosed;
        }

        public void RemoveState(string key)
        {
            CircuitBreakerStateModel stateModel;
            _store.TryRemove(key, out stateModel);
        }

        public void SetLastException(string key, Exception ex)
        {
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                stateModel.LastException = ex;
                _store[key] = stateModel;
            }
        }

        public Exception GetLastException(string key)
        {
            Exception lastException = null;
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                lastException = stateModel.LastException;
            }

            return lastException;
        }

        public void AddStateModel(string key, CircuitBreakerStateModel circuitBreakerStateModel)
        {
            _store.TryAdd(key, circuitBreakerStateModel);
        }
    }
}