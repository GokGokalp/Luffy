using System;

namespace LuffyCore.RetryMechanism
{
    public class RetryMechanismOptions
    {
        public RetryPolicies RetryPolicies { get; set; }
        public int RetryCount { get; set; }
        public TimeSpan Interval { get; set; }

        public RetryMechanismOptions(RetryPolicies retryPolicies, int retryCount, TimeSpan interval)
        {
            RetryPolicies = retryPolicies;
            RetryCount = retryCount;
            Interval = interval;
        }
    }

    public enum RetryPolicies
    {
        Linear,
        Exponential
    }
}