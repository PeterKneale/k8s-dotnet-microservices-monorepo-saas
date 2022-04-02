using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Catalog.Messages;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Catalog.Application
{
    public static class RemoveProduct
    {
        public record Command(string ProductId) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _repository;
            private readonly IPublishEndpoint _publisher;
            private readonly IAccountContextGetter _context;

            public Handler(IProductRepository repository, IPublishEndpoint publisher, IAccountContextGetter context)
            {
                _repository = repository;
                _publisher = publisher;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _repository.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    return Unit.Value;
                }
                
                await _repository.DeleteAsync(product);
                await _publisher.Publish(new ProductDeleted(_context.GetAccountId(), product.ProductId, product.Name, product.Description), cancellationToken);
                
                return Unit.Value;
            }
        }
    }
}