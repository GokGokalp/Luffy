using System;

namespace Luffy.CircuitBreaker
{
    public interface ICircuitBreakerOptions
    {
        int ExceptionThreshold { get; set; }
        int SuccessThresholdWhenCircuitBreakerHalfOpenStatus {get; set;}
        TimeSpan DurationOfBreak { get; set; }
    }
}