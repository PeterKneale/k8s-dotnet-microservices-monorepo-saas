using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Accounts.Api;

namespace Admin.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AccountsPlatformApi.AccountsPlatformApiClient _client;

        public AccountsController(AccountsPlatformApi.AccountsPlatformApiClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index() =>
            View(await _client.ListAccountSummariesAsync(new ListAccountSummariesRequest()));
    }
}