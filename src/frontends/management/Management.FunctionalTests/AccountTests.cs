using System.Threading.Tasks;
using Management.FunctionalTests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class AccountTests : TestBase
    {
        public AccountTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Can_navigate_to_account_management()
        {
            // arrange
            var home = await GotoHomePage();
            
            // act
            await home.Menu.ClickAccount();
        }
    }
}