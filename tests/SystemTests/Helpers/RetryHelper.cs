using System;
using Polly;
using Polly.Retry;

namespace SystemTests.Helpers
{
    public static class RetryHelper
    {
        private const int DefaultRetryIntervalInMilliseconds = 50;
        private const int DefaultNumberOfRetryAttempts = 10;

        public static AsyncRetryPolicy RetryAsync(int milliseconds = DefaultRetryIntervalInMilliseconds, int retries = DefaultNumberOfRetryAttempts) =>
            Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retries,
                    retryAttempt => TimeSpan.FromMilliseconds(milliseconds),
                    (exception, timeSpan, attempts, context) =>
                        Console.WriteLine($"Retry {retries} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));

        public static RetryPolicy Retry(int milliseconds = DefaultRetryIntervalInMilliseconds, int retries = DefaultNumberOfRetryAttempts) =>
            Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    retries,
                    retryAttempt => TimeSpan.FromMilliseconds(milliseconds),
                    (exception, timeSpan, retryCount, context) =>
                        Console.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
    }
}