using System.Threading.Tasks;
using Management.Application;
using Management.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index(string productId)
        {
            var model = new ProductPageModel
            {
                ProductId = productId
            };
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(string productId)
        {
            var result = await _mediator.Send(new GetProduct.Query(productId));
            return PartialView("_Details", result);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string productId)
        {
            var result = await _mediator.Send(new GetProduct.Query(productId));
            var model = new ProductEditPageModel
            {
                ProductId = result.ProductId,
                Name = result.Name,
                Description = result.Description
            };
            return PartialView("_Edit", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditPageModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", model);
            }
            await _mediator.Send(new UpdateProduct.Command(model.ProductId, model.Name, model.Description));
            return base.RedirectToAction(nameof(Details), new { model.ProductId, refresh = true });
        }
    }
}