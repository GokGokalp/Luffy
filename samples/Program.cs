using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuffyCore.CircuitBreaker;
using LuffyCore.RetryMechanism;

namespace LuffyCore.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            double rateWithRetryMechanism = RetryMechanismSample(amount: 1000, from: "USD", to: "EUR").Result;
            double rateWithCircuitBreaker = CircuitBreakerSample(amount: 1000, from: "USD", to: "EUR").Result;

            Console.WriteLine($"Retry works: {rateWithRetryMechanism}");
            Console.WriteLine($"Circuit Breaker works: {rateWithCircuitBreaker}");
            Console.ReadLine();
        }

        static async Task<double> RetryMechanismSample(double amount, string from, string to)
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

        static async Task<double> RetryMechanismWithFallbackSample(double amount, string from, string to)
        {
            double currentRate = await Luffy.Instance
                                .UseRetry(new RetryMechanismOptions(RetryPolicies.Linear,
                                                                    retryCount: 3,
                                                                    interval: TimeSpan.FromSeconds(5)))
                                .ExecuteAsync<double>(async () => {
                                    // Some API calls...
                                    double rate = await CurrencyConverterSampleAPI(amount, from, to);

                                    return rate;
                                }, async () => {
                                    // Some fallback scenario.
                                    double rate = 100;

                                    return await Task.FromResult(rate);                                    
                                });

            return currentRate;
        }

        static async Task<double> CircuitBreakerSample(double amount, string from, string to)
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

        static async Task<double> CurrencyConverterSampleAPI(double amount, string from, string to)
        {
            using(WebClient client = new WebClient())
            {
                string url = $"https://finance.google.com/finance/converter?a={amount}&from={from}&to={to}";
                string response = await client.DownloadStringTaskAsync(url);

                Match match = Regex.Match(response, "<span[^>]*>(.*?)</span>");

                double rate = Convert.ToDouble(match.Groups[1].Value.Replace($" {to}", ""), CultureInfo.InvariantCulture);

                return rate;
            }
        }
    }
}