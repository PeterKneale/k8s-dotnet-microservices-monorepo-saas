using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using MediatR;

namespace Management.Application
{
    public static class CreateCategory
    {
        public record Command(string CategoryId, string Name, string? ParentCategoryId) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly CatalogApi.CatalogApiClient _catalog;

            public Handler(CatalogApi.CatalogApiClient catalog)
            {
                _catalog = catalog;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var request = new AddCategoryRequest
                {
                    CategoryId = command.CategoryId,
                    Name = command.Name,
                    ParentCategoryId = command.ParentCategoryId ?? string.Empty
                };
                await _catalog.AddCategoryAsync(request, cancellationToken: cancellationToken);
                return Unit.Value;
            }
        }

    }
}