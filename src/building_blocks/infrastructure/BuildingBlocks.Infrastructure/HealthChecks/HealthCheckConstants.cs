namespace BuildingBlocks.Infrastructure.HealthChecks
{
    public static class HealthCheckConstants
    {
        public const string HealthCheckAliveRoute = "/health/alive";
        public const string HealthCheckReadyRoute = "/health/ready";
        public const string HealthCheckReadyTag = "ready";
    }
}