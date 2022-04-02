using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Infrastructure.AccountContext;
using FluentValidation;
using MassTransit;
using MediatR;
using Stores.Domain;
using Stores.Messages;

namespace Stores.Application
{
    public static class SetSubdomain
    {
        public record Command(string StoreId, string Subdomain) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.StoreId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Subdomain).NotEmpty().MaximumLength(100);
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
                var storeId = request.StoreId;
                var subdomain = request.Subdomain;

                var store = await _repository.GetByIdAsync(storeId);
                if (store == null)
                {
                    throw new NotFoundException(nameof(Store), storeId);
                }
                
                store.ChangeSubdomain(subdomain);

                await _repository.UpdateAsync(store);

                var message = new StoreUpdated(_context.GetAccountId(), store.StoreId, store.Name, store.Theme, store.Subdomain, store.Domain);
                await _publisher.Publish(message, cancellationToken);

                return Unit.Value;
            }
        }
    }

}