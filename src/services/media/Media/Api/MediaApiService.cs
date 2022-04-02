using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Media.Application;

namespace Media.Api
{
    public class MediaApiService : MediaApi.MediaApiBase
    {
        private readonly IGalleryService _service;
        private readonly IGalleryUploadService _upload;

        public MediaApiService(IGalleryService service, IGalleryUploadService upload)
        {
            _service = service;
            _upload = upload;
        }

        public override async Task<GetPreSignedUrlReply> GetPreSignedUrl(GetPreSignedUrlRequest request, ServerCallContext context)
        {
            var url = await _upload.GenerateForProduct(request.ProductId, request.PictureId);
            return new GetPreSignedUrlReply {Url = url.ToString()};
        }

        public override Task<GetProductGalleryReply> GetProductGallery(GetProductGalleryRequest request, ServerCallContext context)
        {
            var gallery = _service.GetGallery(request.ProductId);

            return Task.FromResult(new GetProductGalleryReply
            {
                Gallery = gallery
            });
        }

        public override Task<GetProductsGalleriesReply> GetProductsGalleries(GetProductsGalleriesRequest request, ServerCallContext context)
        {
            var list = new List<ProductGallery>();
            foreach (var productId in request.ProductIds)
            {
                var gallery = _service.GetGallery(productId);
                list.Add(new ProductGallery {ProductId = productId, Gallery = gallery});
            }
            return Task.FromResult(new GetProductsGalleriesReply {ProductGalleries = {list}});
        }
    }
}