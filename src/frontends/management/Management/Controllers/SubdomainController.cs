using System.Threading.Tasks;
using Management.Application;
using Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stores.Api;

namespace Management.Controllers
{
    public class SubdomainController : Controller
    {
        private readonly StoresApi.StoresApiClient _client;
        private readonly IConfiguration _configuration;

        public SubdomainController(StoresApi.StoresApiClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<PartialViewResult> Details()
        {
            var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});
            var model = new StoreSubdomainViewModel
            {
                Subdomain = store.Subdomain,
                ParentDomain = $".{_configuration.GetParentDomain()}"
            };
            return PartialView("_Details", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> Edit()
        {
            var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});

            var model = new StoreSubdomainEditModel
            {
                Subdomain = store.Subdomain,
                ParentDomain = $".{_configuration.GetParentDomain()}"
            };
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StoreSubdomainEditModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ParentDomain = $".{_configuration.GetParentDomain()}";
                return PartialView("_Edit", model);
            }

            await _client.SetSubdomainAsync(new SetSubdomainRequest {StoreId = User.CurrentStoreId(), Subdomain = model.Subdomain});
            return RedirectToAction(nameof(Details));
        }
    }
}