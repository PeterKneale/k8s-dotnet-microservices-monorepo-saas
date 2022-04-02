namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class Names
    {
        public static readonly string[] FrontendNames = {
            "admin", "management", "marketing", "registration", "shopping"
        };

        public static readonly string[] BackendNames = {
            "registration", "shopping"
        };

        public static readonly string[] ServiceNames = {
            "carts", "catalog", "media", "search", "stores", "accounts"
        };
    }
}