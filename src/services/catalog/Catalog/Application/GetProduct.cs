using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using Catalog.Domain;
using FluentValidation;
using MediatR;

namespace Catalog.Application
{
    public static class GetProduct
    {
        public record Query(string ProductId) : IRequest<Product>;
        
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.ProductId).NotEmpty().MaximumLength(36);
            }
        }
        
        public class Handler : IRequestHandler<Query, Product>
        {
            private readonly IProductRepository _repository;

            public Handler(IProductRepository repository)
            {
                _repository = repository;
            }

            public async Task<Product> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _repository.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    throw new NotFoundException(nameof(Product), request.ProductId);
                }
                return product;
            }
        }
    }
}