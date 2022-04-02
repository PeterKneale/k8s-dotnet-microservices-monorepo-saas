using System;

namespace Registration.Infrastructure
{
    public class Constants
    {
        public const string QueueName = "registrations";
        
        public static Uri QueueUri => new Uri($"queue:{QueueName}");
    }
}