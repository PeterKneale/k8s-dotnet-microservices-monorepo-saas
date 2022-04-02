using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Registration.Api;
using Registration.Models;

namespace Registration.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly RegistrationApi.RegistrationApiClient _client;

        public RegistrationController(RegistrationApi.RegistrationApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var reference = Guid.NewGuid().ToString();
            var model = new RegisterModel {Reference = reference};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _client.SubmitRegistrationAsync(new SubmitRegistrationRequest
            {
                Email = model.Email,
                Name = model.Name,
                Reference = model.Reference
            });
            return RedirectToAction(nameof(Wait), new {model.Reference});
        }

        [HttpGet]
        public async Task<IActionResult> Wait(string reference)
        {
            var status = await _client.GetRegistrationStatusAsync(new GetRegistrationStatusRequest {Reference = reference});
            if (status.Complete)
            {
                return RedirectToAction(nameof(Complete), new {reference});
            }
            return View();
        }
    
        [HttpGet]
        public IActionResult Complete(string reference)
        {
            return View();
        }
    }
}