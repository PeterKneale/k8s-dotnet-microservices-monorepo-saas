using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Carts.Api;
using FluentValidation;
using MediatR;
using Shopping.Api;
using Shopping.Application.Services;

namespace Shopping.Application
{
    public static class GetShoppingCart
    {
        public record Query(string CartId) : IRequest<ShoppingCart>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.CartId).NotEmpty().MaximumLength(36);
            }
        }

        public class Handler : IRequestHandler<Query, ShoppingCart>
        {
            private readonly ICartsServiceGateway _carts;

            public Handler(ICartsServiceGateway carts)
            {
                _carts = carts;
            }

            public async Task<ShoppingCart> Handle(Query request, CancellationToken cancellationToken)
            {
                var cartId = request.CartId;

                var cart = await _carts.GetCartAsync(new GetCartRequest {CartId = cartId},  cancellationToken);

                return new ShoppingCart
                {
                    CartId = cartId,
                    TotalPrice = cart.TotalPrice,
                    Items =
                    {
                        cart.Items.Select(x => new ShoppingCartItem
                        {
                            ProductId = x.ProductId,
                            Description = x.Description,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            UnitPrice = x.UnitPrice
                        })
                    }
                };
            }
        }
    }
}