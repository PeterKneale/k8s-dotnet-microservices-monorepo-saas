namespace Shopping.Infrastructure
{
    public class StoreContext
    {
        public string StoreId { get; init; }
        public string StoreName { get; init; }
        public string StoreTheme { get; init; }
        
        public string AccountId { get; init; }
        public string AccountName { get; init; }
    }
}