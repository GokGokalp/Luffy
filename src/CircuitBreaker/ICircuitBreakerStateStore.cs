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
        CircuitBreakerStateEnum State { get; set; }
        Exception LastException { get; set; }
        DateTime LastStateChangedDateUtc { get; set; }
        bool IsClosed { get; set; }
        int ExceptionAttempt { get; set; }
        int SuccessAttempt { get; set; }
    }
}