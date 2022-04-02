using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Accounts.Api;

namespace Admin.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AccountsApi.AccountsApiClient _client;
        
        public AccountsController(AccountsApi.AccountsApiClient client)
        {
            _client = client;
        }
        
        public async Task<IActionResult> Index() => 
            View(await _client.ListAccountsAsync(new ListAccountRequest()));
    }
}