using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using Media;
using MediatR;

namespace Management.Application
{
    public static class GetProduct
    {
        public record Query(string ProductId) : IRequest<Result>;

        public class Result
        {
            public string CategoryId { get; set; }
            public string ProductId { get; init; }
            public string Name { get; init; }
            public string Description { get; init; }
            public string Price { get; init; }
        }

        public class Validator : AbstractValidator<Query>
        {
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly MediaApi.MediaApiClient _media;
            private readonly CatalogApi.CatalogApiClient _catalog;

            public Handler(CatalogApi.CatalogApiClient catalog, MediaApi.MediaApiClient media)
            {
                _catalog = catalog;
                _media = media;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var reply = await _catalog.GetProductAsync(new GetProductRequest {ProductId = request.ProductId}, cancellationToken: cancellationToken);
                return new Result
                {
                    CategoryId = reply.CategoryId,
                    ProductId = reply.ProductId,
                    Name = reply.Name,
                    Description = reply.Description,
                    Price = $"{reply.Price.DecimalValue.ToString("C")} {reply.Price.CurrencyCode}"
                };
            }

        }
    }
}