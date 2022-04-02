using System.Threading.Tasks;
using Management.Models;
using Microsoft.AspNetCore.Mvc;
using Stores.Api;

namespace Management.Controllers
{
    public class DomainController : Controller
    {
        private readonly StoresApi.StoresApiClient _client;

        public DomainController(StoresApi.StoresApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<PartialViewResult> Details()
        {
            var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});
            var model = new StoreDomainViewModel
            {
                Domain = store.Domain,
                IsDomainValidated = store.DomainValidated
            };
            return PartialView("_Details", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> Edit()
        {
            var store = await _client.GetStoreAsync(new GetStoreRequest {StoreId = User.CurrentStoreId()});
            var model = new StoreDomainEditModel
            {
                Domain = store.Domain,
                Validated = store.DomainValidated
            };
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StoreDomainEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", model);
            }
            await _client.SetDomainAsync(new SetDomainRequest {StoreId = User.CurrentStoreId(), Domain = model.Domain});
            return RedirectToAction(nameof(Details));
        }
    }
}