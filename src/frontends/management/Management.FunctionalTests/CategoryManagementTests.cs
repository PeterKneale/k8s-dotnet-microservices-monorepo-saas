using System.Threading.Tasks;
using FluentAssertions;
using Management.FunctionalTests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class CategoryManagementTests: TestBase
    {
        public CategoryManagementTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
        
        [Fact]
        public async Task Can_create()
        {
            // arrange
            var home = await GotoHomePage();
            var page = await home.Menu.ClickCategories();

            // act
            await page.ClickCreate();
            await page.EnterName("Cars");
            await page.ClickSave();
            // assert
            (await page.GetCategoryName()).Should().Be("Cars");
            (await page.GetCategoryNamePath()).Should().Be("Cars");
        }
        
        [Fact]
        public async Task Can_edit()
        {
            // arrange
            var home = await GotoHomePage();
            var page = await home.Menu.ClickCategories();
            await page.ClickCreate();
            await page.EnterName("Cars");
            await page.ClickSave();
            
            // act
            await page.SelectCategory("Cars");
            await page.ClickEdit();
            await page.EnterName("Cars Updated");
            await page.ClickSave();
            
            // assert
            (await page.GetCategoryName()).Should().Be("Cars Updated");
            (await page.GetCategoryNamePath()).Should().Be("Cars Updated");
        }
        
        [Fact]
        public async Task Can_delete()
        {
            // arrange
            var home = await GotoHomePage();
            var page = await home.Menu.ClickCategories();
            await page.ClickCreate();
            await page.EnterName("Cars");
            await page.ClickSave();

            // act
            await page.SelectCategory("Cars");
            await page.ClickDelete();
            
            // assert
            // todo: how to assert?
        }
    }
}
