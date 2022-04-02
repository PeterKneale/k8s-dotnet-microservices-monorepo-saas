using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using MediatR;

namespace Management.Application
{
    public static class ListCategories
    {
        public record Query(string? CategoryId = null) : IRequest<Result>;

        public class Result
        {
            public IEnumerable<CategoryViewModel> Categories { get; init; }

            public class CategoryViewModel
            {
                public string CategoryId { get; init; }
                public string Name { get; init; }
                public int Level { get; set; }
                public bool Selected { get; set; }
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
                var categories = await GetCategories(cancellationToken);
                var models = MapToCategoryViewModels(categories);
                foreach (var model in models)
                    if (model.CategoryId == request.CategoryId)
                        model.Selected = true;
                var list = new List<Result.CategoryViewModel>();
                list.AddRange(models);
                return new Result
                {
                    Categories = list
                };
            }

            private async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken)
            {
                var request = new ListCategoriesRequest();
                var reply = await _catalog.ListCategoriesAsync(request, cancellationToken: cancellationToken);
                return reply.Categories;
            }

            private IEnumerable<Result.CategoryViewModel> MapToCategoryViewModels(IEnumerable<Category> categories) => categories.Select(x => new Result.CategoryViewModel
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Level = x.Level
            });
        }
    }
}