using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shopping.Api;

namespace Shopping.Infrastructure
{
    public interface IStoreContextLoader
    {
        Task<StoreContext> GetStoreContext(string domain);
    }
    
    public class StoreContextLoader : IStoreContextLoader
    {
        private readonly ShoppingApi.ShoppingApiClient _client;
        private readonly ILogger<StoreContextLoader> _logs;

        public StoreContextLoader(ShoppingApi.ShoppingApiClient client, ILogger<StoreContextLoader> logs)
        {
            _client = client;
            _logs = logs;
        }

        public async Task<StoreContext> GetStoreContext(string domain)
        {
            _logs.LogInformation($"Getting store context for domain {domain}");
            var store = await _client.GetStoreAsync(new GetStoreRequest {Domain = domain});

            return new StoreContext
            {
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                StoreTheme = store.StoreTheme,
                AccountId = store.AccountId,
                AccountName = store.AccountName
            };
        }
    }
}