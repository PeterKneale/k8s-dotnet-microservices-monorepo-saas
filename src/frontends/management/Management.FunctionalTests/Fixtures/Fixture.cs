using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace Management.FunctionalTests.Fixtures
{
    public class Fixture :  IDisposable
    {
        private readonly IPlaywright _playwright;
        private readonly IBrowser _browser;
        private IBrowserContext? _context;
        
        public Fixture()
        {
            var options = new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 10,
                Timeout = 5000
            };

            _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
            _browser = _playwright.Chromium.LaunchAsync(options).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _context?.CloseAsync().GetAwaiter().GetResult();
            _browser.CloseAsync().GetAwaiter().GetResult();
            _browser.DisposeAsync().GetAwaiter().GetResult();
            _playwright.Dispose();
        }
        
        public async Task<IPage> GetNewPage()
        {
            if (_context != null)
            {
                await _context.CloseAsync();
                await _context.DisposeAsync();
            }
            _context = await _browser.NewContextAsync();
            return await _context.NewPageAsync();
        }
    }
}