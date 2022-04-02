namespace BuildingBlocks.Infrastructure.AccountContext.Strategies
{
    public class ScopedGetter : IAccountContextGetter
    {
        private readonly IScopedAccountContextStore _store;
        
        public ScopedGetter(IScopedAccountContextStore store)
        {
            _store = store;
        }
        
        public string GetAccountId() => _store.GetAccountId() ?? throw new AccountContextMissingException();
    }
}