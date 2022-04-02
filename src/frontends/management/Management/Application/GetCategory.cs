using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using Media;
using MediatR;

namespace Management.Application
{
    public static class GetCategory
    {
        public record Query(string CategoryId) : IRequest<Result>;

        public class Result
        {
            public CategoryViewModel Category { get; init; }

            public class CategoryViewModel
            {
                public string CategoryId { get; init; }
                public string Name { get; init; }
                public string NamePath { get; init; }
                public string ParentCategoryId { get; init; }
            }
        }

        public class Validator : AbstractValidator<Query>
        {
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly CatalogApi.CatalogApiClient _catalog;

            public Handler(CatalogApi.CatalogApiClient catalog)
            {
                _catalog = catalog;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await GetCategory(request.CategoryId, cancellationToken);
                return new Result
                {
                    Category = new Result.CategoryViewModel
                    {
                        CategoryId = category.CategoryId,
                        ParentCategoryId = category.ParentCategoryId,
                        Name = category.Name,
                        NamePath = category.NamePath
                    }
                };
            }

            private async Task<Category> GetCategory(string categoryId, CancellationToken cancellationToken)
            {
                var request = new GetCategoryRequest {CategoryId = categoryId};
                var reply = await _catalog.GetCategoryAsync(request, cancellationToken: cancellationToken);
                return reply;
            }
        }
    }
}