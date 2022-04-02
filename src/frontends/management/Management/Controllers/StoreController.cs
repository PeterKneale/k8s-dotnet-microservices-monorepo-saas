using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}