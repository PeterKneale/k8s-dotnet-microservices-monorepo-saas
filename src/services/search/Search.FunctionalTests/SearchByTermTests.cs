using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using FluentAssertions;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Messages;
using MassTransit;
using Search.Api;
using Xunit;
using Xunit.Abstractions;

namespace Search.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class SearchByTermTests
    {
        private readonly SearchApi.SearchApiClient _client;
        private readonly IBus _bus;

        public SearchByTermTests(Fixture api, ITestOutputHelper output)
        {
            api.OutputHelper = output;
            _client = api.GetClient();
            _bus = api.Services.GetRequiredService<IBus>();
        }

        [Fact]
        public async Task PublishProductAdded_IndexEmpty_ProductFoundInIndex()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, accountId)};
                    
            var names = GetProductNames();
            var categoryId = Guid.NewGuid().ToString();
            string categoryName = "cars";

            // Act
            foreach (var name in names)
            {
                var productId = Guid.NewGuid().ToString();
                string? description = null;

                var message = new ProductAdded(accountId, productId, name, description, categoryId, categoryName, categoryId, categoryName);
                await _bus.Publish(message);
            }

            // Assert
            foreach (var name in names)
            {
                await Retry.RetryAsync.ExecuteAsync(async () => {
                    // Search
                    var response = await _client.SearchAsync(new SearchRequest {Query = name, CategoryIdPath = categoryId, PageOffset = 0, PageSize = 1}, headers);

                    // Assert
                    response.Results.Should().ContainSingle(x => x.Name == name);
                });
            }
        }

        private static string[] GetProductNames() => new[]
        {
            "Aston Martin", "Audi", "Bentley", "BMW", "Bugatti", "Cadillac",
            "Chevrolet", "Chrysler", "Dodge", "Ferrari", "Fiat", "Ford", "Honda", "Hyundai", "Jaguar",
            "Jeep", "Kia", "Lamborghini", "Land Rover", "Maserati", "Mazda", "Mercedes Benz", "Mini",
            "Nissan", "Polestar", "Porsche", "Rolls Royce", "Smart", "Tesla", "Toyota", "Volkswagen",
            "Volvo"
        };
    }
}