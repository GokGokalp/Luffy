using System;

namespace LuffyCore.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        public int ExceptionAttempt {get; set;}
        public int SuccessAttempt {get; set;}
        public CircuitBreakerStateEnum State {get; set;}
        public Exception LastException {get; set;}
        public DateTime LastStateChangedDateUtc {get; set;}
        public bool IsClosed {get; set;}
    }
}