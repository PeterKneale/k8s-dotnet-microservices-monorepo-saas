using System.Collections.Generic;
using System.Threading.Tasks;
using Management.FunctionalTests.Pages.PageElements;
using Microsoft.Playwright;

namespace Management.FunctionalTests.Pages
{
    public class ProductPage
    {
        public readonly IPage _page;

        public ProductPage(IPage page)
        {
            _page = page;
        }
        public MenuPageElement Menu => new MenuPageElement(_page);
        
        private ILocator List => _page.Locator("id=list");
        private ILocator ListItem(string name) => List.Locator($"text={name}");
        private ILocator Pane => _page.Locator("id=pane");
        private ILocator CreateButton => _page.Locator(".btn >> text=Create Product");
        private ILocator EditButton => Pane.Locator(".btn >> text=Edit Product");
        private ILocator DeleteButton => Pane.Locator(".btn >> text=Delete Product");
        private ILocator CancelButton => Pane.Locator(".btn >> text=Cancel");
        private ILocator SaveButton => Pane.Locator(".btn >> text=Save");
        private ILocator NameField => Pane.Locator("input[name=\"Name\"]");
        private ILocator DescriptionField => Pane.Locator("input[name=\"Description\"]");
        
        private ILocator ProductName => Pane.Locator("[data-test-id=\"product-name\"]");
        private ILocator ProductDescription=> Pane.Locator("[data-test-id=\"product-description\"]");

        public async Task<string> GetProductName() => await ProductName.InnerTextAsync();
        public async Task<string> GetProductDescription() => await ProductDescription.InnerTextAsync();
        
        public async Task<IEnumerable<string>> GetProductNames() => await ProductName.AllInnerTextsAsync();
            
        public async Task EnterName(string text)
        {
            await NameField.ClickAsync();
            await NameField.FillAsync(text);
        }
        public async Task EnterDescription(string text)
        {
            await DescriptionField.ClickAsync();
            await DescriptionField.FillAsync(text);
        }

        public async Task ClickCreate() => await CreateButton.ClickAsync();
        public async Task ClickEdit() => await EditButton.ClickAsync();
        public async Task ClickSave() => await SaveButton.ClickAsync();
        public async Task ClickCancel() => await CancelButton.ClickAsync();

        public async Task SelectCategory(string name) => await ListItem(name).ClickAsync();

        public async Task ClickDelete()
        {
            _page.Dialog += (_, dialog) =>
            {
                dialog.AcceptAsync();
            };
            await DeleteButton.ClickAsync();
        }
    }
}