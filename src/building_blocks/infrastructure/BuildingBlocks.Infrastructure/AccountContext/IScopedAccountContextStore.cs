namespace BuildingBlocks.Infrastructure.AccountContext
{
    public interface IScopedAccountContextStore
    {
        string? GetAccountId();
        void SetAccountId(string accountId);
    }
}