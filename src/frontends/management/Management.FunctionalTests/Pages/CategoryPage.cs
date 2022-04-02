using System.Threading.Tasks;
using Management.FunctionalTests.Pages.PageElements;
using Microsoft.Playwright;

namespace Management.FunctionalTests.Pages
{
    public class CategoryPage
    {
        public readonly IPage _page;

        public CategoryPage(IPage page)
        {
            _page = page;
        }
        
        public MenuPageElement Menu => new MenuPageElement(_page);
        
        private ILocator List => _page.Locator("id=list");
        private ILocator ListItem(string name) => List.Locator($"text={name}");
        private ILocator Pane => _page.Locator("id=pane");
        private ILocator CreateButton => _page.Locator(".btn >> text=Create Category");
        private ILocator CreateChildButton => Pane.Locator(".btn >> text=Create Child Category");
        private ILocator EditButton => Pane.Locator(".btn >> text=Edit Category");
        private ILocator DeleteButton => Pane.Locator(".btn >> text=Delete Category");
        private ILocator SaveButton => Pane.Locator(".btn >> text=Save");
        private ILocator NameField => Pane.Locator("input[name=\"Name\"]");
        
        private ILocator CategoryName => Pane.Locator("[data-test-id=\"category-name\"]");
        private ILocator CategoryNamePath => Pane.Locator("[data-test-id=\"category-name-path\"]");

        public async Task<string> GetCategoryName() => await CategoryName.InnerTextAsync();
        public async Task<string> GetCategoryNamePath() => await CategoryNamePath.InnerTextAsync();
            
        public async Task EnterName(string text)
        {
            await NameField.ClickAsync();
            await NameField.FillAsync(text);
        }

        public async Task ClickCreate() => await CreateButton.ClickAsync();
        public async Task ClickCreateChild() => await CreateChildButton.ClickAsync();
        public async Task ClickEdit() => await EditButton.ClickAsync();
        public async Task ClickSave() => await SaveButton.ClickAsync();

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