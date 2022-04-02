using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Carts.Domain;
using FluentValidation;
using MediatR;

namespace Carts.Application
{
    public static class RemoveProductFromCart
    {
        public record Command(string CartId, string ProductId) : IRequest;
        
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CartId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
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
                var cart = await _repository.GetByIdAsync(request.CartId);
                if (cart == null)
                {
                    throw new NotFoundException(nameof(cart), request.CartId);
                }
                if (!cart.ContainsProduct(request.ProductId))
                {
                    // Idempotent
                    return Unit.Value;
                }
                cart.RemoveProduct(request.ProductId);
                await _repository.UpdateAsync(cart);
                return Unit.Value;
            }
        }
    }
}