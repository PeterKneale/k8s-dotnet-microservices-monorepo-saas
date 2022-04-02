using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Shopping.Infrastructure
{
    public interface IContextualMiddlewareService
    {
        Task SetContext(HttpContext httpContext);
    }
    
    public class ContextualMiddlewareService : IContextualMiddlewareService
    {
        private readonly IStoreContextLoader _storeContextLoader;
        private readonly IDomainNameGetter _domainNameGetter;
        private readonly IStoreContextSetter _storeContextSetter;
        private readonly IAccountContextSetter _accountContextSetter;

        public ContextualMiddlewareService(
            IStoreContextLoader storeContextLoader,
            IDomainNameGetter domainNameGetter,
            IStoreContextSetter storeContextSetter,
            IAccountContextSetter accountContextSetter)
        {
            _storeContextLoader = storeContextLoader;
            _domainNameGetter = domainNameGetter;
            _storeContextSetter = storeContextSetter;
            _accountContextSetter = accountContextSetter;
        }

        public async Task SetContext(HttpContext httpContext)
        {
            var domain = _domainNameGetter.GetDomainName();
            var context = await _storeContextLoader.GetStoreContext(domain);

            _storeContextSetter.SetStoreContext(context);
            _accountContextSetter.SetAccountId(context.AccountId);
        }
    }
}