using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Infrastructure.AccountContext;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Stores.Application.Services;
using Stores.Domain;
using Stores.Infrastructure.Repository;
using Stores.Messages;

namespace Stores.Application
{
    public static class AddStore
    {
        public record Command(string StoreId, string Name) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.StoreId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IStoreRepository _repository;
            private readonly IPublishEndpoint _publisher;
            private readonly IAccountContextGetter _context;

            public Handler(IStoreRepository repository, IPublishEndpoint publisher, IAccountContextGetter context)
            {
                _repository = repository;
                _publisher = publisher;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var accountId = _context.GetAccountId();
                var storeId = request.StoreId;
                var storeName = request.Name;
                
                // idempotent
                if (await _repository.ExistsByIdAsync(storeId))
                {
                    return Unit.Value;
                }
                
                var subdomain = storeName.GenerateSlug();

                // Domain names must be unique
                if (await _repository.ExistsDefaultDomainAsync(subdomain))
                {
                    throw new AlreadyExistsException(nameof(Store), subdomain);
                }
                
                var store = new Store(storeId, storeName, subdomain);

                await _repository.CreateAsync(store);

                var message = new StoreAdded(accountId, store.StoreId, store.Name, store.Theme, store.Subdomain);
                await _publisher.Publish(message, cancellationToken);

                return Unit.Value;
            }
        }
    }

}