using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using Catalog.Domain;
using FluentValidation;
using MediatR;

namespace Catalog.Application
{
    public static class GetCategory
    {
        public record Query(string CategoryId) : IRequest<Category>;
        
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.CategoryId).NotEmpty().MaximumLength(36);
            }
        }
        
        public class Handler : IRequestHandler<Query, Category>
        {
            private readonly ICategoryRepository _repository;

            public Handler(ICategoryRepository repository)
            {
                _repository = repository;
            }

            public async Task<Category> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await _repository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    throw new NotFoundException(nameof(Category), request.CategoryId);
                }
                return category;
            }
        }
    }
}