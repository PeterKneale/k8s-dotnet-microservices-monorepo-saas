using System;
using System.Diagnostics;
using Polly;
using Polly.Retry;

namespace Registration.FunctionalTests
{
    public static class Retry
    {
        private const int RetryCount = 30;
        private static TimeSpan RetryDelay = TimeSpan.FromMilliseconds(1000);

        public static RetryPolicy RetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(retryCount: RetryCount, retryAttempt => RetryDelay, 
                (exception, timeSpan, retryCount, context) => 
                    Trace.WriteLine($"Retry {retryCount} encountered error {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
    }
}