using System;

namespace Luffy
{
    public interface ICircuitBreakerOptions
    {
        int ExceptionThreshold { get; set; }
        int SuccessThresholdWhenCircuitBreakerHalfOpenStatus {get; set;}
        TimeSpan DurationOfBreak { get; set; }
    }
}