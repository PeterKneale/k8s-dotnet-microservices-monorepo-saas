using System.Threading.Tasks;
using Management.Models;
using Media;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class GalleryController : Controller
    {
        private readonly MediaApi.MediaApiClient _client;

        public GalleryController(MediaApi.MediaApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<PartialViewResult> Details(string productId)
        {
            var result = await _client.GetProductGalleryAsync(new GetProductGalleryRequest {ProductId = productId});

            var model = new GalleryDetailsViewModel
            {
                ProductId = productId,
                PictureUrls = result.Gallery.PictureUrls
            };

            return PartialView("_Details", model);
        }

        [HttpGet]
        public PartialViewResult Upload(string productId)
        {
            var model = new GalleryUploadViewModel {ProductId = productId};

            return PartialView("_Upload", model);
        }

        [HttpGet]
        public async Task<ContentResult> PreSignedUrl(string productId, string name)
        {
            var result = await _client.GetPreSignedUrlAsync(new GetPreSignedUrlRequest
            {
                ProductId = productId,
                PictureId = name
            });
            return Content(result.Url);
        }
    }
}