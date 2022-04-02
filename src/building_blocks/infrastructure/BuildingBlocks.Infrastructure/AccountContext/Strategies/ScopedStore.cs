namespace BuildingBlocks.Infrastructure.AccountContext.Strategies
{
    public class ScopedStore : IScopedAccountContextStore
    {
        private string? _accountId;
        
        public string? GetAccountId() => _accountId;
        public void SetAccountId(string accountId) => _accountId = accountId;
    }
}