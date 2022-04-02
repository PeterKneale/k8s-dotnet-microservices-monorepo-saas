using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using Carts.Domain;
using FluentValidation;
using MediatR;

namespace Carts.Application
{
    public static class GetCart
    {
        public record Query(string CartId) : IRequest<Cart>;
        
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.CartId).NotEmpty().MaximumLength(36);
            }
        }
        
        public class Handler : IRequestHandler<Query, Cart>
        {
            private readonly ICartRepository _repository;

            public Handler(ICartRepository repository)
            {
                _repository = repository;
            }

            public async Task<Cart> Handle(Query request, CancellationToken cancellationToken)
            {
                var cart = await _repository.GetByIdAsync(request.CartId);
                if (cart == null)
                {
                    throw new NotFoundException(nameof(cart), request.CartId);
                }
                return cart;
            }
        }
    }
}