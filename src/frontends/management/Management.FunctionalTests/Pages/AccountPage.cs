using Management.FunctionalTests.Pages.PageElements;
using Microsoft.Playwright;

namespace Management.FunctionalTests.Pages
{
    public class AccountPage
    {
        private readonly IPage _page;

        public AccountPage(IPage page)
        {
            _page = page;
        }
        
        public MenuPageElement Menu => new MenuPageElement(_page);
    }
}