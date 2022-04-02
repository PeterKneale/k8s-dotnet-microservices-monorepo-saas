using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using Google.Type;
using MediatR;

namespace Management.Application
{
    public static class CreateProduct
    {
        public record Command(string CategoryId, string ProductId, string Name, string? Description) : IRequest;

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
                var request = new AddProductRequest
                {
                    CategoryId = command.CategoryId,
                    ProductId = command.ProductId,
                    Name = command.Name,
                    Description = command.Description ?? string.Empty,
                    Price = new Money{DecimalValue = 1, CurrencyCode = "AUD"}
                };
                await _catalog.AddProductAsync(request, cancellationToken: cancellationToken);
                return Unit.Value;
            }
        }

    }
}