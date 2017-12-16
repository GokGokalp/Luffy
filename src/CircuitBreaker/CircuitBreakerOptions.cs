using System;

namespace LuffyCore.CircuitBreaker
{
    public class CircuitBreakerOptions
    {
        public string Key { get; set; }
        public int ExceptionThreshold {get; set;}
        public int SuccessThresholdWhenCircuitBreakerHalfOpenStatus { get; set; }
        public TimeSpan DurationOfBreak {get; set;}

        public CircuitBreakerOptions(string key, int exceptionThreshold, int successThresholdWhenCircuitBreakerHalfOpenStatus, TimeSpan durationOfBreak)
        {
            Key = key;
            ExceptionThreshold = exceptionThreshold;
            SuccessThresholdWhenCircuitBreakerHalfOpenStatus = successThresholdWhenCircuitBreakerHalfOpenStatus;
            DurationOfBreak = durationOfBreak;
        }
    }
}