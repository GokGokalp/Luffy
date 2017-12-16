using System;
using System.Collections.Generic;

namespace LuffyCore.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private readonly Dictionary<string, CircuitBreakerStateModel> _store = new Dictionary<string, CircuitBreakerStateModel>();

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
            bool isClosed = false;
            CircuitBreakerStateModel stateModel;
            if(_store.TryGetValue(key, out stateModel))
            {
                isClosed = stateModel.IsClosed;
            }

            return isClosed;
        }

        public void RemoveState(string key)
        {
            _store.Remove(key);
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
    }

    public class CircuitBreakerStateModel
    {
        public CircuitBreakerStateEnum State {get; set;}
        public int ExceptionAttempt {get; set;}
        public int SuccessAttempt {get; set;}
        public Exception LastException {get; set;}
        public DateTime LastStateChangedDateUtc {get; set;}
        public bool IsClosed {get; set;}
    }
}