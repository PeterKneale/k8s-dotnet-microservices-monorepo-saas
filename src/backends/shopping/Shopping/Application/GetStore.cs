using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using FluentValidation;
using MediatR;
using Shopping.Application.Data;
using Shopping.Infrastructure.DataSources;
using Store=Shopping.Api.Store;

namespace Shopping.Application
{
    public static class GetStore
    {
        public record Query(string Domain) : IRequest<Store>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Domain).NotEmpty().MaximumLength(1024);
            }
        }

        public class Handler : IRequestHandler<Query, Store>
        {
            private readonly IDataReader _reader;
            
            public Handler(IDataReader reader)
            {
                _reader = reader;
            }

            public async Task<Store> Handle(Query request, CancellationToken cancellationToken)
            {
                var domain = request.Domain;
                
                var store = await GetStore(domain);

                var accountId = store.AccountId;
                var account = await _reader.GetAccountById(accountId);
                if (account == null)
                {
                    throw new NotFoundException(nameof(account), accountId);
                }
                
                return new Store
                {
                    StoreId = store.StoreId,
                    StoreName = store.Name,
                    StoreTheme = store.Theme,
                    AccountId = accountId,
                    AccountName = account.Name
                };
            }
            
            private async Task<StoreData?> GetStore(string? domain)
            {
                var store = await _reader.GetStoreByDomain(domain);
                if (store != null)
                {
                    return store;
                }
                
                // todo: Refactor
                var subdomain = domain.Replace(".ecommerce-store-builder.dev", string.Empty);
                store = await _reader.GetStoreBySubdomain(subdomain);
                if (store != null)
                {
                    return store;
                }
                
                throw new NotFoundException(nameof(store), domain);
            }
        }
    }
}