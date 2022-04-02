using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.Configuration;
using SystemTests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SystemTests
{
    public class HealthTests : TestBase, IClassFixture<Fixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly IHttpClientFactory _clients;
        
        private static string AliveEndpoint => "/health/alive";
        private static string ReadyEndpoint => "/health/ready";
        private static TimeSpan Timeout => TimeSpan.FromMilliseconds(250);

        public HealthTests(Fixture container, ITestOutputHelper output) : base(container)
        {
            _output = output;
            _clients = container.HttpClientFactory;
        }
        
        [Fact]
        public async Task Verify_ingress_is_alive() => 
            await TestAlive("nginx", ConfigurationHelper.Configuration.GetNginxUri());
        
        [Fact]
        public async Task Verify_ingress_is_ready() => 
            await TestReady("nginx", ConfigurationHelper.Configuration.GetNginxUri());
        
        [Theory]
        [MemberData(nameof(FrontendNames))]
        public async Task Verify_all_frontends_are_alive(string name) => 
            await TestAlive(name, ConfigurationHelper.Configuration.GetFrontendHttpUri(name));

        [Theory]
        [MemberData(nameof(BackendNames))]
        public async Task Verify_all_backends_are_alive(string name) => 
            await TestAlive(name, ConfigurationHelper.Configuration.GetBackendHttpUri(name));

        [Theory]
        [MemberData(nameof(ServiceNames))]
        public async Task Verify_all_services_are_alive(string name) => 
            await TestAlive(name, ConfigurationHelper.Configuration.GetServiceHttpUri(name));
        
        [Theory]
        [MemberData(nameof(FrontendNames))]
        public async Task Verify_all_frontends_are_ready(string name) => 
            await TestReady(name, ConfigurationHelper.Configuration.GetFrontendHttpUri(name));

        [Theory]
        [MemberData(nameof(BackendNames))]
        public async Task Verify_all_backends_are_ready(string name) => 
            await TestReady(name, ConfigurationHelper.Configuration.GetBackendHttpUri(name));

        [Theory]
        [MemberData(nameof(ServiceNames))]
        public async Task Verify_all_services_are_ready(string name) => 
            await TestReady(name, ConfigurationHelper.Configuration.GetServiceHttpUri(name));

        private async Task TestAlive(string name, Uri uri) => await TestEndpoint(name, uri, AliveEndpoint);
        private async Task TestReady(string name, Uri uri) => await TestEndpoint(name, uri, ReadyEndpoint);

        private async Task TestEndpoint(string name, Uri uri, string endpoint)
        {
            var client = _clients.CreateClient();
            client.BaseAddress = uri;
            client.Timeout = Timeout;
            await RetryHelper.RetryAsync().ExecuteAsync(async () => {
                _output.WriteLine($"Checking {name} is alive at {client.BaseAddress}...");
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
            });
        }

        public static IEnumerable<object[]> ServiceNames => Names.ServiceNames.Select(item => new object[] {item});
        public static IEnumerable<object[]> BackendNames => Names.BackendNames.Select(item => new object[] {item});
        public static IEnumerable<object[]> FrontendNames => Names.FrontendNames.Select(item => new object[] {item});
    }
}