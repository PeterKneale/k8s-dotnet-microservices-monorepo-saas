using System.Threading.Tasks;
using Management.FunctionalTests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class StoreTests : TestBase
    {
        public StoreTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Can_navigate_to_store_management()
        {
            // arrange
            var home = await GotoHomePage();
            
            // act
            await home.Menu.ClickStore();
        }
    }
}