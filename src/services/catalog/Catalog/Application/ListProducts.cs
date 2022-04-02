using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain;
using FluentValidation;
using MediatR;

namespace Catalog.Application
{
    public static class ListProducts
    {
        public record Query(string? CategoryId = null) : IRequest<IEnumerable<Product>>;
        
        public class Validator : AbstractValidator<Query>
        {
        }
        
        public class Handler : IRequestHandler<Query, IEnumerable<Product>>
        {
            private readonly IProductRepository _repository;

            public Handler(IProductRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.ListAsync(request.CategoryId);
            }
        }
    }
}