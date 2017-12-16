#   **Luffy**
------------------------------

![alt tag](https://raw.githubusercontent.com/GokGokalp/Luffy/master/img/logo.jpg)

Luffy is a simple resilience and transient-fault handling library for .NET

#### Features:
- Luffy provides circuit breaker feature
- Luffy provides retry mechanism with back-off (linear and exponentially)

#### Usages:
-----
Sample usage for the circuit breaker:

```cs
async Task<double> CircuitBreakerSample(double amount, string from, string to)
{
    double currentRate = await Luffy.Instance
                        .UseCircuitBreaker(new CircuitBreakerOptions(key: "CurrencyConverterSampleAPI",
                                                                     exceptionThreshold: 5,
                                                                     successThresholdWhenCircuitBreakerHalfOpenStatus: 5,
                                                                     durationOfBreak: TimeSpan.FromSeconds(5)))
                        .ExecuteAsync<double>(async () => {
                            // Some API calls...
                            double rate = await CurrencyConverterSampleAPI(amount, from, to);

                            return rate;
                        });

    return currentRate;
}
```

Sample usage for the retry mechanism:

```cs
async Task<double> RetryMechanismSample(double amount, string from, string to)
{
    double currentRate = await Luffy.Instance
                        .UseRetry(new RetryMechanismOptions(RetryPolicies.Linear,
                                                            retryCount: 3,
                                                            interval: TimeSpan.FromSeconds(5)))
                        .ExecuteAsync<double>(async () => {
                            // Some API calls...
                            double rate = await CurrencyConverterSampleAPI(amount, from, to);

                            return rate;
                        });

    return currentRate;
}
```

#### Samples:
- [Luffy.Sample]

[Luffy.Sample]: https://github.com/GokGokalp/Luffy/tree/master/samples