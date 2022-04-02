using System;
using System.Linq;
using System.Threading.Tasks;
using Carts.Application;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Grpc.Core;
using MediatR;

namespace Carts.Api
{
    public class CartsApiService : CartsApi.CartsApiBase
    {
        private readonly IMediator _mediator;

        public CartsApiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Empty> AddProductToCart(AddProductToCartRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddProductToCart.Command(request.CartId, request.ProductId, request.Description, request.Quantity, request.UnitPrice.DecimalValue, request.UnitPrice.CurrencyCode));
            return new Empty();
        }

        public override async Task<Empty> RemoveProductFromCart(RemoveProductFromCartRequest request, ServerCallContext context)
        {
            await _mediator.Send(new RemoveProductFromCart.Command(request.CartId, request.ProductId));
            return new Empty();
        }

        public override async Task<Cart> GetCart(GetCartRequest request, ServerCallContext context)
        {
            var cart = await _mediator.Send(new GetCart.Query(request.CartId));
            var model = new Cart
            {
                CartId = cart.CartId,
                TotalPrice = new Money {DecimalValue = cart.TotalPrice.Amount, CurrencyCode = cart.TotalPrice.Currency.CurrencyCode},
                Items =
                {
                    cart.Products.Select(x => new CartItem
                    {
                        ProductId = x.ProductId,
                        Description = x.Description,
                        Quantity = x.Quantity,
                        Price = new Money {DecimalValue = x.GetPrice().Amount, CurrencyCode = x.GetPrice().Currency.CurrencyCode},
                        UnitPrice = new Money {DecimalValue = x.UnitPrice.Amount, CurrencyCode = x.UnitPrice.Currency.CurrencyCode},
                    })
                }
            };
            return model;
        }

        public override Task<Empty> AdjustProductQuantityInCart(AdjustProductQuantityInCartRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}