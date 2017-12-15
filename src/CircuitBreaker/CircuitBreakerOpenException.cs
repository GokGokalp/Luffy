using System;

namespace LuffyCore.CircuitBreaker
{
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(string message, Exception ex)
            :base(message, ex)
        {
            
        }
    }
}