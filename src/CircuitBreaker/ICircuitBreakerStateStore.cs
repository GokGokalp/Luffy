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
        void AddStateModel(string key, CircuitBreakerStateModel circuitBreakerStateModel);
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

    public class CircuitBreakerStateModel
    {
        public CircuitBreakerStateEnum State { get; set; }
        public int ExceptionAttempt { get; set; }
        public int SuccessAttempt { get; set; }
        public Exception LastException { get; set; }
        public DateTime LastStateChangedDateUtc { get; set; }
        public bool IsClosed { get; set; }
    }
}