using System;
using System.Diagnostics;
using Polly;
using Polly.Retry;

namespace Shopping.FunctionalTests
{
    public static class Retry
    {
        private const int RetryCount = 10;
        private static TimeSpan RetryDelay = TimeSpan.FromMilliseconds(1001);

        public static AsyncRetryPolicy RetryAsync = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryCount: RetryCount, retryAttempt => RetryDelay, 
                (exception, timeSpan, retryCount, context) => 
                Trace.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
    }
}