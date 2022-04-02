using System;
using System.Threading.Tasks;
using Management.FunctionalTests.Fixtures;
using Management.FunctionalTests.Pages;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    public abstract class TestBase
    {
        protected TestBase(Fixture fixture, ITestOutputHelper output)
        {
            Fixture = fixture;
            Output = output;
        }
        
        protected Fixture Fixture { get; }
        
        protected ITestOutputHelper Output{ get; }
        
        protected async Task<HomePage> GotoHomePage()
        {
            var uri = new Uri(Configuration.RootUri, "/Account/Login");
            
            var newPage = await Fixture.GetNewPage();
            await newPage.GotoAsync(uri.ToString());

            var login = new LoginPage(newPage, Output);
            var home = await login.Login("test@example.com", "password");
            return home;
        }
    }
}