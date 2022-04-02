using System;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Api;
using SystemTests.Helpers;

namespace SystemTests
{
    public class TestBase
    {
        private readonly Fixture _container;

        protected TestBase(Fixture container)
        {
            _container = container;
            CatalogService = ServiceProvider.GetRequiredService<CatalogApi.CatalogApiClient>();
        }
        
        protected CatalogApi.CatalogApiClient CatalogService { get; }

        protected IServiceProvider ServiceProvider => _container.ServiceProvider;
    }
}