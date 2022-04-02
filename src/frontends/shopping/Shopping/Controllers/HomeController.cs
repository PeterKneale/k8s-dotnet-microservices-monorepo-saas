using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Api;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShoppingApi.ShoppingApiClient _client;

        public HomeController(ShoppingApi.ShoppingApiClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string? query, int offset=0)
        {
            var request = new SearchProductsRequest {Query = query?? string.Empty, PageSize = 10, PageOffset = offset};
            var response = await _client.SearchProductsAsync(request);
            return PartialView(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}