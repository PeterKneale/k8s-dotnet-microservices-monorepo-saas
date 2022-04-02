using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Shopping.Controllers;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{
    public class Fixture : WebApplicationFactory<HomeController>, ITestOutputHelperAccessor
    {
        public ITestOutputHelper? OutputHelper { get; set; }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(p => p.AddXUnit(this));
        }
    }
}