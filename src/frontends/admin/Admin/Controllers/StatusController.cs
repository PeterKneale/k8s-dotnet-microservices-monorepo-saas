using System;
using System.Net.Http;
using System.Threading.Tasks;
using Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class StatusController : Controller
    {
        private readonly HttpClient _http;

        public StatusController(HttpClient http)
        {
            _http = http;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<PartialViewResult> Check(Uri uri, string type)
        {
            try
            {
                _http.Timeout = TimeSpan.FromSeconds(3);
                var response = await _http.GetStringAsync(uri);
                return PartialView("Partials/Check", new StatusModel {Success = true, Message = response, Type = type, Uri = uri});
            }
            catch (Exception e)
            {
                return PartialView("Partials/Check", new StatusModel {Success = false, Message = e.Message, Type = type, Uri = uri});
            }
        }
    }
}