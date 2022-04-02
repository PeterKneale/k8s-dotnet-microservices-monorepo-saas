using System;
using System.Threading.Tasks;
using Management.Application;
using Management.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> ListCategories()
        {
            var categories = await _mediator.Send(new ListCategories.Query());
            return PartialView("_ListCategories", new CategoryListPageModel(categories));
        }
        
        [HttpGet]
        public async Task<IActionResult> ListProducts(string? categoryId)
        {
            var products = await _mediator.Send(new ListProducts.Query(categoryId));
            return PartialView("_ListProducts", products);
        }

        [HttpGet]
        public IActionResult Create(string categoryId)
        {
            var model = new ProductsCreatePageModel
            {
                ProductId = Guid.NewGuid().ToString(),
                CategoryId = categoryId
            };
            return PartialView("_Create", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(ProductsCreatePageModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }
            await _mediator.Send(new CreateProduct.Command(model.CategoryId, model.ProductId, model.Name, model.Description));
            return base.RedirectToAction(nameof(ListProducts), new { model.ProductId });
        }
    }
}