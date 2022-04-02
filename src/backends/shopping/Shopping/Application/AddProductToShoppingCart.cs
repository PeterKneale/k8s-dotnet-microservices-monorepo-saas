using System.Threading;
using System.Threading.Tasks;
using Carts.Api;
using FluentValidation;
using MediatR;
using Catalog.Api;
using Shopping.Application.Services;

namespace Shopping.Application
{
    public static class AddProductToShoppingCart
    {
        public record Command(string CartId, string ProductId, int Quantity) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CartId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Quantity).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICatalogServiceGateway _catalog;
            private readonly ICartsServiceGateway _carts;

            public Handler(ICatalogServiceGateway catalog, ICartsServiceGateway carts)
            {
                _catalog = catalog;
                _carts = carts;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var productId = command.ProductId;
                var cartId = command.CartId;
                var quantity = command.Quantity;

                var product = await _catalog.GetProductAsync(new GetProductRequest
                {
                    ProductId = productId
                }, cancellationToken);

                var unitPrice = product.Price;
                var description = product.Name;

                await _carts.AddProductToCartAsync(new AddProductToCartRequest
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Description = description
                }, cancellationToken);

                return Unit.Value;
            }

        }
    }

}