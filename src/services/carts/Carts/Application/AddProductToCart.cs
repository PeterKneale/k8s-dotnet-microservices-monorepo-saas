using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DDD.ValueTypes;
using Carts.Domain;
using FluentValidation;
using MediatR;

namespace Carts.Application
{
    public static class AddProductToCart
    {
        public record Command(string CartId, string ProductId, string Description, int Quantity, decimal UnitPrice, string CurrencyCode) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CartId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Quantity).GreaterThan(0);
                RuleFor(x => x.UnitPrice).GreaterThan(0);
                RuleFor(x => x.CurrencyCode).NotEmpty().MaximumLength(3);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICartRepository _repository;

            public Handler(ICartRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency = Currency.FromCurrencyCode(request.CurrencyCode);
                var unitPrice = Money.CreateInstance(request.UnitPrice, currency);
                
                var cart = await _repository.GetByIdAsync(request.CartId);
                if (cart == null)
                {
                    cart = Cart.CreateInstance(request.CartId, currency);
                    cart.AddProduct(request.ProductId, request.Description, request.Quantity, unitPrice);
                    await _repository.SaveAsync(cart);
                    return Unit.Value;
                }
                
                cart.AddProduct(request.ProductId, request.Description, request.Quantity, unitPrice);
                await _repository.UpdateAsync(cart);
                return Unit.Value;
            }
        }
    }

}