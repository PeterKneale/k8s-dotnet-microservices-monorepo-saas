using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.Configuration;
using FluentAssertions;
using SystemTests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SystemTests
{
    public class IngressTests : TestBase, IClassFixture<Fixture>
    {
        private readonly ITestOutputHelper _output;

        public IngressTests(Fixture container, ITestOutputHelper output) : base(container)
        {
            _output = output;
        }

        [Theory]
        [InlineData("marketing.saas.io", "/")]                  // Allows anonymous access to home page
        [InlineData("registration.saas.io", "/")]               // Allows anonymous access to home page
        [InlineData("management.saas.io", "/account/login")]    // Redirects to login page
        [InlineData("shopping.saas.io", "/")]                   // Allows anonymous access to home page
        [InlineData("admin.saas.io", "/account/login")]         // Redirects to login page
        public async Task Locations_are_returned_by_nginx(string host, string url)
        {
            // arrange
            var address = ConfigurationHelper.Configuration.GetNginxUri();

            var client = new HttpClient();
            _output.WriteLine($"Requesting {host}{url} at address {address}");
            client.BaseAddress = address;
            client.DefaultRequestHeaders.Host = host;

            // act
            var response = await client.GetAsync(url);

            // debug
            _output.WriteLine($"Response Status {response.StatusCode}");
            foreach (var (key, value) in response.Headers)
            {
                var values = string.Join(',', value);
                _output.WriteLine($"Response Header {key} = {values}");
            }
            
            // assert
            response.Headers.Server.Should().ContainSingle("nginx");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task Ingress_fallback_page()
        {
            // arrange
            var address = ConfigurationHelper.Configuration.GetNginxUri();
            var host = "does-not-exist.saas.io";
            var url = "/";

            var client = new HttpClient();
            _output.WriteLine($"Requesting {host}{url} at address {address}");
            client.BaseAddress = address;
            client.DefaultRequestHeaders.Host = host;

            // act
            var response = await client.GetAsync(url);

            // debug
            _output.WriteLine($"Response Status {response.StatusCode}");
            foreach (var (key, value) in response.Headers)
            {
                var values = string.Join(',', value);
                _output.WriteLine($"Response Header {key} = {values}");
            }
            
            // assert
            response.Headers.Server.Should().ContainSingle("nginx");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Sorry, page not found");
        }
    }
}