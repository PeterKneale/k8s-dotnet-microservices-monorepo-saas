using System.Linq;
using System.Threading.Tasks;
using Management.Application;
using Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Stores.Api;

namespace Management.Controllers
{
    public class ThemeController : Controller
    {
        private readonly StoresApi.StoresApiClient _client;
        private readonly IConfiguration _configuration;

        public ThemeController(StoresApi.StoresApiClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<PartialViewResult> Details()
        {
            var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});
            var model = new StoreThemeViewModel
            {
                CurrentTheme = store.Theme
            };
            return PartialView("_Details", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> Edit()
        {
            var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});
            var themes = _configuration.GetThemes();
            var model = new StoreThemeEditModel
            {
                CurrentTheme = store.Theme,
                Themes = themes.Select(text => new SelectListItem(text, text, text == store.Theme))
            };
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StoreThemeEditModel model)
        {
            if (!ModelState.IsValid)
            {
                var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});
                var themes = _configuration.GetThemes();
                model.Themes = themes.Select(text => new SelectListItem(text, text, text == store.Theme));
                return PartialView("_Edit", model);
            }
            await _client.SetThemeAsync(new SetThemeRequest {StoreId = User.CurrentStoreId(), Theme = model.CurrentTheme});
            return RedirectToAction(nameof(Details));
        }
    }
}