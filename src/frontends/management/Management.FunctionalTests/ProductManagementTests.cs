using System.Threading.Tasks;
using FluentAssertions;
using Management.FunctionalTests.Fixtures;
using Management.FunctionalTests.Pages;
using Xunit;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class ProductManagementTests : TestBase
    {
        public ProductManagementTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Can_create()
        {
            // arrange
            var home = await GotoHomePage();
            var categories = await home.Menu.ClickCategories();
            await categories.CreateCategories(new[] {"Category 1", "Category 2"});
            
            var page = await categories.Menu.ClickProducts();
            
            // act
            await page.SelectCategory("Category 1");
            await page.ClickCreate();
            await page.EnterName("Product 1 Name");
            await page.ClickSave();
            
            // assert
            (await page.GetProductName()).Should().Be("Product 1 Name");
            (await page.GetProductDescription()).Should().BeEmpty();
        }
        
        [Fact]
        public async Task Can_update()
        {
            // arrange
            var home = await GotoHomePage();
            var categories = await home.Menu.ClickCategories();
            await categories.CreateCategories(new[] {"Category 1", "Category 2"});
            
            var page = await categories.Menu.ClickProducts();
            await page.SelectCategory("Category 1");
            await page.ClickCreate();
            await page.EnterName("Product 1 Name");
            await page.EnterDescription("Product 1 Description");
            await page.ClickSave();
            
            // act
            await page.ClickEdit();
            await page.EnterName("Product 1 Name Updated");
            await page.EnterDescription("Product 1 Description Updated");
            await page.ClickSave();
            
            // assert
            (await page.GetProductName()).Should().Be("Product 1 Name Updated");
            (await page.GetProductDescription()).Should().Be("Product 1 Description Updated");
        }
        
        [Fact]
        public async Task Can_cancel()
        {
            // arrange
            var home = await GotoHomePage();
            var categories = await home.Menu.ClickCategories();
            await categories.CreateCategories(new[] {"Category 1", "Category 2"});
            
            var page = await categories.Menu.ClickProducts();
            await page.SelectCategory("Category 1");
            await page.ClickCreate();
            await page.EnterName("Product Name");
            await page.EnterDescription("Product Description");
            await page.ClickSave();
            
            // act
            await page.ClickEdit();
            await page.EnterName("xxxxxxxx");
            await page.EnterDescription("xxxxxxxx");
            await page.ClickCancel();
            
            // assert
            (await page.GetProductName()).Should().Be("Product Name");
            (await page.GetProductDescription()).Should().Be("Product Description");
        }
    }
}