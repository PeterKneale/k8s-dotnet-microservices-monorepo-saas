using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class ShoppingTests
    {
        private readonly Fixture _fixture;
        public ShoppingTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            fixture.OutputHelper = outputHelper;
        }

        [Fact(Skip = "Need to mock downstream dependencies")]
        public async Task LoadingNonExistentStoreShouldRedirect()
        {
            // Arrange
            using var client = _fixture.CreateDefaultClient();
            client.DefaultRequestHeaders.Host = $"{Guid.NewGuid()}.saas.io";
                
            // Act
            var response = await client.GetAsync(new Uri("/", UriKind.Relative));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}