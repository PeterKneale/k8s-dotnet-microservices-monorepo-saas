using System.Threading.Tasks;
using FluentAssertions;
using Management.FunctionalTests.Fixtures;
using Management.FunctionalTests.Pages;
using Xunit;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class ProductListTests : TestBase
    {
        public ProductListTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Can_list_all_products()
        {
            // arrange
            var home = await GotoHomePage();
            var categories = await home.Menu.ClickCategories();
            await categories.CreateCategories(new[] {"Category 1", "Category 2"});

            // act
            var products = await home.Menu.ClickProducts();
            await products.CreateProductsInCategory("Category 1", new[] {"Product 1 Name", "Product 2 Name"});
            await products.CreateProductsInCategory("Category 2", new[] {"Product 3 Name", "Product 4 Name"});

            // assert
            var page = await home.Menu.ClickProducts();
            
            (await page.GetProductNames()).Should().BeEquivalentTo("Product 1 Name", "Product 2 Name", "Product 3 Name", "Product 4 Name");
        }
        
        [Fact]
        public async Task Can_list_products_by_category()
        {
            // arrange
            var home = await GotoHomePage();
            var categories = await home.Menu.ClickCategories();
            await categories.CreateCategories(new[] {"Category 1", "Category 2"});

            // act
            var products = await home.Menu.ClickProducts();
            await products.CreateProductsInCategory("Category 1", new[] {"Product 1 Name", "Product 2 Name"});
            await products.CreateProductsInCategory("Category 2", new[] {"Product 3 Name", "Product 4 Name"});

            // assert
            var page = await home.Menu.ClickProducts();
            
            await page.SelectCategory("Category 1");
            (await page.GetProductNames()).Should().BeEquivalentTo("Product 1 Name", "Product 2 Name");
            
            await page.SelectCategory("Category 2");
            (await page.GetProductNames()).Should().BeEquivalentTo("Product 3 Name", "Product 4 Name");
        }
    }
}