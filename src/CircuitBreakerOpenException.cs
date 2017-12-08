using System;

namespace Luffy
{
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(string message, Exception ex)
            :base(message, ex)
        {
            
        }
    }
}