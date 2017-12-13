using System;

namespace Luffy.CircuitBreaker
{
    public enum CircuitBreakerStateEnum
    {
        Open,
        HalfOpen,
        Closed
    }

    public interface ICircuitBreakerStateStore
    {
        CircuitBreakerStateEnum State { get; set; }
        Exception LastException { get; set; }
        DateTime LastStateChangedDateUtc { get; set;}
        void Trip(Exception ex);
        void Reset();
        void HalfOpen();
        bool IsClosed { get; }
    }
}