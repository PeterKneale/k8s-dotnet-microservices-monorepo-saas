using Management.FunctionalTests.Pages.PageElements;
using Microsoft.Playwright;

namespace Management.FunctionalTests.Pages
{
    public class HomePage
    {
        private readonly IPage _page;

        public HomePage(IPage page)
        {
            _page = page;
        }
        
        public MenuPageElement Menu => new MenuPageElement(_page);
    }
}