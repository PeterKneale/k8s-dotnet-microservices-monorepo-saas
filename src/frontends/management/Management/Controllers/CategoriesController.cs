using System;
using System.Threading.Tasks;
using Management.Application;
using Management.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List(string? categoryId = null)
        {
            var request = new ListCategories.Query(categoryId);
            var categories = await _mediator.Send(request);
            return PartialView("_List", new CategoryListPageModel(categories));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string categoryId, bool refresh = false)
        {
            var result = await _mediator.Send(new GetCategory.Query(categoryId));
            var model = new CategoryDetailsPageModel
            {
                CategoryId = result.Category.CategoryId,
                Name = result.Category.Name,
                NamePath = result.Category.NamePath,
                ParentCategoryId = result.Category.ParentCategoryId
            };
            if (refresh)
            {
                AddListRefreshHeader();
            }
            return PartialView("_Details", model);
        }

        [HttpGet]
        public IActionResult Create(string? parentCategoryId = null)
        {
            var model = new CategoryCreatePageModel
            {
                ParentCategoryId = parentCategoryId
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreatePageModel model)
        {
            string categoryId = Guid.NewGuid().ToString();
            await _mediator.Send(new CreateCategory.Command(
                categoryId,
                model.Name,
                model.ParentCategoryId));
            return base.RedirectToAction(nameof(Details), new {categoryId, refresh = true});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string categoryId)
        {
            await _mediator.Send(new DeleteCategory.Command(categoryId));
            AddListRefreshHeader();
            return Content("Category deleted");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string categoryId)
        {
            var result = await _mediator.Send(new GetCategory.Query(categoryId));
            var model = new CategoryEditPageModel
            {
                CategoryId = result.Category.CategoryId,
                Name = result.Category.Name
            };
            return PartialView("_Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditPageModel model)
        {
            await _mediator.Send(new UpdateCategory.Command(model.CategoryId, model.Name));
            return RedirectToAction(nameof(Details), new {model.CategoryId, refresh = true});
        }

        private void AddListRefreshHeader() => Response.Headers.Add("HX-Trigger", "list-refresh");
    }
}