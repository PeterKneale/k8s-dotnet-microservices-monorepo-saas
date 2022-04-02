﻿using Management.FunctionalTests.Pages.PageElements;
using Microsoft.Playwright;

namespace Management.FunctionalTests.Pages
{
    public class StorePage
    {
        private readonly IPage _page;

        public StorePage(IPage page)
        {
            _page = page;
        }
        
        public MenuPageElement Menu => new MenuPageElement(_page);
    }
}