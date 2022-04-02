using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Catalog.Messages;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Catalog.Application
{
    public static class UpdateProductDetails
    {
        public record Command(string ProductId, string Name, string? Description) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Description).MaximumLength(200);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _products;
            private readonly ICategoryRepository _categories;
            private readonly IPublishEndpoint _publisher;
            private readonly IAccountContextGetter _context;

            public Handler(IProductRepository products, ICategoryRepository categories, IPublishEndpoint publisher, IAccountContextGetter context)
            {
                _products = products;
                _categories = categories;
                _publisher = publisher;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _products.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    throw new NotFoundException(nameof(Product), request.ProductId);
                }
                var category = await _categories.GetByIdAsync(product.CategoryId);
                if (category == null)
                {
                    throw new NotFoundException(nameof(Category), product.CategoryId);
                }

                product.ChangeDetails(request.Name, request.Description);

                await _products.UpdateAsync(product);
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