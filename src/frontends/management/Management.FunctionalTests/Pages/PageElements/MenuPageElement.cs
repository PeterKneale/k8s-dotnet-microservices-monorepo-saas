using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Management.FunctionalTests.Pages.PageElements
{
    public class MenuPageElement
    {
        private readonly IPage _page;

        public MenuPageElement(IPage page)
        {
            _page = page;
        }

        public async Task<HomePage> ClickHome()
        {
            await _page.Locator(".nav-link >> text=Home").ClickAsync();
            return new HomePage(_page);
        }
        
        public async Task<CategoryPage> ClickCategories()
        {
            await _page.Locator(".nav-link >> text=Categories").ClickAsync();
            return new CategoryPage(_page);
        }
        
        public async Task<ProductPage> ClickProducts()
        {
            await _page.Locator(".nav-link >> text=Products").ClickAsync();
            return new ProductPage(_page);
        }
        
        public async Task<StorePage> ClickStore()
        {
            await _page.Locator(".nav-link >> text=Store").ClickAsync();
            return new StorePage(_page);
        }
        
        public async Task<AccountPage> ClickAccount()
        {
            await _page.Locator(".nav-link >> text=Account").ClickAsync();
            return new AccountPage(_page);
        }
    }
}