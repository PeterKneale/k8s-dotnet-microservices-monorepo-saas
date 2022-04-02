using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Application.Data;
using Shopping.Infrastructure.DataSources;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class ResolutionTests
    {
        private readonly Fixture _fixture;

        public ResolutionTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            fixture.OutputHelper = outputHelper;
        }

        [Fact]
        public void Can_resolve_root_IDocumentStore()
        {
            // act
            var service = _fixture.Services.GetRequiredService<IDocumentStore>();
            // assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Can_resolve_scoped_IQuerySession()
        {
            // arrange 
            using var scope = _fixture.Services.CreateScope();
            // act
            var service = scope.ServiceProvider.GetRequiredService<IQuerySession>();
            // assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Can_resolve_scoped_IDataWriter()
        {
            // arrange 
            using var scope = _fixture.Services.CreateScope();
            // act
            var service = scope.ServiceProvider.GetRequiredService<IDataWriter>();
            // assert
            service.Should().NotBeNull();
        }
        
        [Fact]
        public void Can_resolve_scoped_IDataReader()
        {
            // arrange 
            using var scope = _fixture.Services.CreateScope();
            // act
            var service = scope.ServiceProvider.GetRequiredService<IDataReader>();
            // assert
            service.Should().NotBeNull();
        }
    }
}