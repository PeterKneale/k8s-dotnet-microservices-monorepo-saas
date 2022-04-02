using System.Threading.Tasks;
using FluentAssertions;
using Management.FunctionalTests.Fixtures;
using Management.FunctionalTests.Pages;
using Xunit;
using Xunit.Abstractions;

namespace Management.FunctionalTests
{
    [Collection(nameof(Fixtures.Fixture))]
    public class CategoryHierarchyTests : TestBase
    {
        public CategoryHierarchyTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Can_create_a_hierarchy()
        {
            // arrange
            var home = await GotoHomePage();
            var page = await home.Menu.ClickCategories();
            
            // act
            await CreateCategory(page, "Food");
            await CreateChildCategory(page, "Food", "Burgers");
            await CreateChildCategory(page, "Food", "Fries");
            await CreateChildCategory(page, "Food", "Fish");
            await CreateChildCategory(page, "Food", "Chips");
            await CreateCategory(page, "Drinks");
            await CreateChildCategory(page, "Drinks", "Coffee");
            await CreateChildCategory(page, "Drinks", "Tea");
            await CreateChildCategory(page, "Drinks", "Beer");
            
            // assert
            await AssertCategoryNamePath(page, "Burgers", "Food,Burgers");
            await AssertCategoryNamePath(page, "Fries", "Food,Fries");
            await AssertCategoryNamePath(page, "Coffee", "Drinks,Coffee");
            await AssertCategoryNamePath(page, "Tea", "Drinks,Tea");
        }

        private static async Task AssertCategoryNamePath(CategoryPage page, string name, string expectedNamePath)
        {
            await page.SelectCategory(name);
            var namePath = await page.GetCategoryNamePath();
            namePath.Should().Be(expectedNamePath);
        }

        private static async Task CreateChildCategory(CategoryPage page, string parent, string child)
        {
            await page.SelectCategory(parent);
            await page.ClickCreateChild();
            await page.EnterName(child);
            await page.ClickSave();
        }

        private static async Task CreateCategory(CategoryPage page, string name)
        {
            await page.ClickCreate();
            await page.EnterName(name);
            await page.ClickSave();
        }
    }
}