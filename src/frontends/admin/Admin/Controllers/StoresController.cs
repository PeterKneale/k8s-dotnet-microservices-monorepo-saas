using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stores.Api;

namespace Admin.Controllers
{
    public class StoresController : Controller
    {
        private readonly StoresApi.StoresApiClient _client;
        
        public StoresController(StoresApi.StoresApiClient client)
        {
            _client = client;
        }
        
        public async Task<IActionResult> Index() => 
            View(await _client.ListStoresAsync(new ListStoresRequest()));
    }
}