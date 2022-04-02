namespace BuildingBlocks.Infrastructure.AccountContext.Strategies
{
    public class ScopedSetter : IAccountContextSetter
    {
        private readonly IScopedAccountContextStore _store;
        
        public ScopedSetter(IScopedAccountContextStore store)
        {
            _store = store;
        }

        public void SetAccountId(string accountId) => _store.SetAccountId(accountId);
    }
}