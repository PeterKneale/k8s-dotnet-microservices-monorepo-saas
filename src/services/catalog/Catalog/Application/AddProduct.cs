using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Catalog.Messages;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Catalog.Application
{
    public static class AddProduct
    {
        public record Command(string ProductId, string CategoryId, string Name, string? Description, decimal Price, string CurrencyCode) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.CategoryId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Description).MaximumLength(200);
                RuleFor(x => x.Price).GreaterThan(0);
                RuleFor(x => x.CurrencyCode).NotEmpty().MaximumLength(3);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _repository;
            private readonly ICategoryRepository _categories;
            private readonly IPublishEndpoint _publisher;
            private readonly IAccountContextGetter _context;

            public Handler(IProductRepository repository, ICategoryRepository categories, IPublishEndpoint publisher, IAccountContextGetter context)
            {
                _repository = repository;
                _categories = categories;
                _publisher = publisher;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _repository.GetByIdAsync(request.ProductId);
                if (product != null)
                {
                    return Unit.Value;
                }

                var category = await _categories.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    throw new NotFoundException(nameof(Category), request.CategoryId);
                }

                product = new Product(request.ProductId, request.CategoryId, request.Name, request.Description, request.Price, request.CurrencyCode);

                await _repository.SaveAsync(product);
                await Publish(product, category, cancellationToken);

                return Unit.Value;
            }

            private async Task Publish(Product product, Category category, CancellationToken cancellationToken) =>
                await _publisher.Publish(
                    new ProductAdded(
                        _context.GetAccountId(),
                        product.ProductId,
                        product.Name,
                        product.Description,
                        product.CategoryId,
                        product.Name,
                        category.IdPath,
                        category.NamePath), cancellationToken);
        }
    }

}