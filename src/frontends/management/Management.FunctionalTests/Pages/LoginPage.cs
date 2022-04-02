using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit.Abstractions;

namespace Management.FunctionalTests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly ITestOutputHelper? _output;

        public LoginPage(IPage page, ITestOutputHelper? output = null)
        {
            _page = page;
            _output = output;
        }

        public async Task<HomePage> Login(string email, string password)
        {
            var accountId = Guid.NewGuid().ToString();
            var storeId = Guid.NewGuid().ToString();
            _output?.WriteLine($"Logging in with {email} and {password} on account {accountId}");
            await _page.Locator("input[name=\"email\"]").ClickAsync();
            await _page.Locator("input[name=\"email\"]").FillAsync(email);
            await _page.Locator("input[name=\"email\"]").PressAsync("Tab");
            await _page.Locator("input[name=\"password\"]").FillAsync(password);
            await _page.Locator("input[name=\"password\"]").PressAsync("Tab");
            await _page.Locator("input[name=\"accountId\"]").FillAsync(accountId);
            await _page.Locator("input[name=\"storeId\"]").FillAsync(storeId);
            await _page.Locator("text=Login").ClickAsync();
            return new HomePage(_page);
        }
    }

}