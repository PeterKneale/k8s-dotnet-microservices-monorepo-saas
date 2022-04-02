using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using MediatR;

namespace Management.Application
{
    public static class UpdateProduct
    {
        public record Command(string ProductId, string Name, string? Description) : IRequest;

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
                var request = new UpdateProductDetailsRequest
                {
                    ProductId = command.ProductId,
                    Name = command.Name,
                    Description = command.Description ?? string.Empty
                };
                
                await _catalog.UpdateProductDetailsAsync(request, cancellationToken: cancellationToken);
                return Unit.Value;
            }
        }
    }
}