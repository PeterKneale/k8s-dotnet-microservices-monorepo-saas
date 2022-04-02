using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using MediatR;

namespace Management.Application
{
    public static class UpdateCategory
    {
        public record Command(string CategoryId, string Name) : IRequest;

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
                var request = new UpdateCategoryRequest
                {
                    CategoryId = command.CategoryId,
                    Name = command.Name
                };
                await _catalog.UpdateCategoryAsync(request, cancellationToken: cancellationToken);
                return Unit.Value;
            }
        }
    }
}