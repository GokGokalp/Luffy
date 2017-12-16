using System;

namespace LuffyCore.CircuitBreaker 
{
    public enum CircuitBreakerStateEnum 
    {
        Open,
        HalfOpen,
        Closed
    }

    public interface ICircuitBreakerStateStore {
        DateTime GetLastStateChangedDateUtc(string key);
        void ChangeLastStateChangedDateUtc(string key, DateTime date);
        bool IsClosed(string key);
        void ChangeState(string key, CircuitBreakerStateEnum state);
        int GetExceptionAttempt(string key);
        void IncreaseExceptionAttemp(string key);
        int GetSuccessAttempt(string key);
        void IncreaseSuccessAttemp(string key);
        void RemoveState(string key);
        Exception GetLastException(string key);
        void SetLastException(string key, Exception ex);
    }
}