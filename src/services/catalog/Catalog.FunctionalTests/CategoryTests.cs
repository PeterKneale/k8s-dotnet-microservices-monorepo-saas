using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Catalog.Api;
using Catalog.Infrastructure;
using FluentAssertions;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class CategoryTests
    {
        private readonly CatalogApi.CatalogApiClient _client;
        private static Metadata MetaData => new() { new Metadata.Entry(MetaDataConstants.AccountId, "1") };

        public CategoryTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task GetCategory_CategoryDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();

            // Act
            Func<Task> act = async () =>
            {
                await _client.GetCategoryAsync(new GetCategoryRequest { CategoryId = productId }, MetaData);
            };

            // Assert
            var ex = await act.Should().ThrowAsync<RpcException>();
            ex.And.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }

        [Fact]
        public async Task AddCategory_CategoryDoesNotExist_ReturnsOK()
        {
            // Arrange
            var categoryId = Guid.NewGuid().ToString();
            var parentCategoryId = string.Empty;
            var name = "x";

            // Act
            await _client.AddCategoryAsync(new AddCategoryRequest
            {
                CategoryId = categoryId,
                ParentCategoryId = parentCategoryId,
                Name = name
            }, MetaData);

            // Assert
            var product = await _client.GetCategoryAsync(new GetCategoryRequest { CategoryId = categoryId }, MetaData);
            product.CategoryId.Should().Be(categoryId);
            product.ParentCategoryId.Should().Be(parentCategoryId);
            product.Name.Should().Be(name);
            product.Level.Should().Be(0);
            product.IdPath.Should().Be(categoryId);
            product.NamePath.Should().Be(name);
        }

        [Fact]
        public async Task RemoveCategory_CategoryExists_ReturnsOK()
        {
            // Arrange
            var categoryId = Guid.NewGuid().ToString();
            var parentCategoryId = string.Empty;
            var name = "x";

            // Act
            await _client.AddCategoryAsync(new AddCategoryRequest
            {
                CategoryId = categoryId,
                ParentCategoryId = parentCategoryId,
                Name = name
            }, MetaData);
            await _client.DeleteCategoryAsync(new DeleteCategoryRequest { CategoryId = categoryId }, MetaData);

            // Assert
            Func<Task> act = async () =>
            {
                await _client.GetCategoryAsync(new GetCategoryRequest { CategoryId = categoryId }, MetaData);
            };
            var ex = await act.Should().ThrowAsync<RpcException>();
            ex.And.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateCategory_CategoryExists_ReturnsOK()
        {
            // Arrange
            var categoryId = Guid.NewGuid().ToString();
            var parentCategoryId = string.Empty;
            var name1 = "x";
            var name2 = "y";

            // Act
            await _client.AddCategoryAsync(new AddCategoryRequest
            {
                CategoryId = categoryId,
                ParentCategoryId = parentCategoryId,
                Name = name1
            }, MetaData);
            await _client.UpdateCategoryAsync(new UpdateCategoryRequest
            {
                CategoryId = categoryId,
                Name = name2
            }, MetaData);

            // Assert
            var category = await _client.GetCategoryAsync(new GetCategoryRequest { CategoryId = categoryId }, MetaData);
            category.Name.Should().Be(name2);
        }


        [Fact]
        public async Task AddCategoryTree_CategoriesDoNotExist_ReturnsOK()
        {
            // Arrange
            async Task CreateNode(string name, string id, string? parent = null) =>
                await _client.AddCategoryAsync(new AddCategoryRequest
                {
                    CategoryId = id,
                    ParentCategoryId = parent ?? string.Empty,
                    Name = name
                }, MetaData);

            // Act
            await CreateNode("A", "a");
            await CreateNode("B", "b");
            await CreateNode("C", "c");
            await CreateNode("A 1", "a_1", "a");
            await CreateNode("A 2", "a_2", "a");
            await CreateNode("A 3", "a_3", "a");
            await CreateNode("B 1", "b_1", "b");
            await CreateNode("B 2", "b_2", "b");
            await CreateNode("B 3", "b_3", "b");
            await CreateNode("A 1 1", "a_1_i", "a_1");
            await CreateNode("A 1 2", "a_1_ii", "a_1");
            await CreateNode("A 1 3", "a_1_iii", "a_1");

            // Assert
            var results = await _client.ListCategoriesAsync(new ListCategoriesRequest(), MetaData);

            var a = results.Categories.Single(x => x.CategoryId == "a");
            a.ParentCategoryId.Should().BeEmpty();
            a.Level.Should().Be(0);
            a.IdPath.Should().Be("a");
            a.NamePath.Should().Be("A");

            var a_1 = results.Categories.Single(x => x.CategoryId == "a_1");
            a_1.ParentCategoryId.Should().Be("a");
            a_1.Level.Should().Be(1);
            a_1.IdPath.Should().Be("a,a_1");
            a_1.NamePath.Should().Be("A,A 1");

            var a_1_iii = results.Categories.Single(x => x.CategoryId == "a_1_iii");
            a_1_iii.ParentCategoryId.Should().Be("a_1");
            a_1_iii.Level.Should().Be(2);
            a_1_iii.IdPath.Should().Be("a,a_1,a_1_iii");
            a_1_iii.NamePath.Should().Be("A,A 1,A 1 3");
        }
    }
}