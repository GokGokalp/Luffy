using System;

namespace Luffy
{
    public class CircuitBreakerOptions : ICircuitBreakerOptions
    {
        public int ExceptionThreshold {get; set;}
        public int SuccessThresholdWhenCircuitBreakerHalfOpenStatus { get; set; }
        public TimeSpan DurationOfBreak {get; set;}

        public CircuitBreakerOptions(int exceptionThreshold, int successThresholdWhenCircuitBreakerHalfOpenStatus, TimeSpan durationOfBreak)
        {
            ExceptionThreshold = exceptionThreshold;
            SuccessThresholdWhenCircuitBreakerHalfOpenStatus = successThresholdWhenCircuitBreakerHalfOpenStatus;
            DurationOfBreak = durationOfBreak;
        }
    }
}