using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using MediatR;

namespace Management.Application
{
    public static class DeleteCategory
    {
        public record Command(string CategoryId) : IRequest;

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
                var request = new DeleteCategoryRequest
                {
                    CategoryId = command.CategoryId
                };
                await _catalog.DeleteCategoryAsync(request, cancellationToken: cancellationToken);
                return Unit.Value;
            }
        }

    }
}