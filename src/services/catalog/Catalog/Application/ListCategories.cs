using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain;
using FluentValidation;
using MediatR;

namespace Catalog.Application
{
    public static class ListCategories
    {
        public record Query() : IRequest<IEnumerable<Category>>;
        
        public class Validator : AbstractValidator<Query>
        {
        }
        
        public class Handler : IRequestHandler<Query, IEnumerable<Category>>
        {
            private readonly ICategoryRepository _repository;

            public Handler(ICategoryRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Category>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.ListAsync();
            }
        }
    }
}