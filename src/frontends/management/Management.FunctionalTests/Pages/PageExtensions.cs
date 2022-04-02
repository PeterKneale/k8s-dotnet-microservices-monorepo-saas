using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.FunctionalTests.Pages
{
    public static class PageExtensions
    {
        public static async Task CreateProductsInCategory(this ProductPage page, string categoryName, IEnumerable<string> productNames)
        {
            foreach (var productName in productNames)
            {
                await page.SelectCategory(categoryName);
                await page.ClickCreate();
                await page.EnterName(productName);
                await page.ClickSave();
            }
        }

        public static async Task CreateCategories(this CategoryPage page, IEnumerable<string> categoryNames)
        {
            foreach (var categoryName in categoryNames)
            {
                await page.ClickCreate();
                await page.EnterName(categoryName);
                await page.ClickSave();
            }
        }
    }
}