using System;

namespace Luffy.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private readonly ICircuitBreakerOptions _circuitBreakerOptions;
        private int _exceptionAttempt;
        private int _successAttempt;
        public CircuitBreakerStateEnum State {get; set;}
        public Exception LastException {get; set;}
        public DateTime LastStateChangedDateUtc {get; set;}

        public CircuitBreakerStateStore(ICircuitBreakerOptions circuitBreakerOptions)
        {
            _circuitBreakerOptions = circuitBreakerOptions;
        }

        public bool IsClosed {get; set;}

        public void HalfOpen()
        {
            State = CircuitBreakerStateEnum.HalfOpen;
            LastStateChangedDateUtc = DateTime.UtcNow;
        }

        public void Reset()
        {
            if(_successAttempt >= _circuitBreakerOptions.SuccessThresholdWhenCircuitBreakerHalfOpenStatus)
            {
                IsClosed = true;
                _exceptionAttempt = 0;
                _successAttempt = 0;
            }
        }

        public void Trip(Exception ex)
        {
            _exceptionAttempt++;

            if(_exceptionAttempt >= _circuitBreakerOptions.ExceptionThreshold)
            {                
                LastException = ex;
                State = CircuitBreakerStateEnum.Open;
                LastStateChangedDateUtc = DateTime.UtcNow;
            }
        }
    }
}