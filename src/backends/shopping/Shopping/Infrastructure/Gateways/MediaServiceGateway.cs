using System.Threading.Tasks;
using Media;
using Shopping.Application.Services;

namespace Shopping.Infrastructure.Gateways
{
    public class MediaServiceGateway : IMediaServiceGateway
    {
        private readonly MediaApi.MediaApiClient _client;
        
        public MediaServiceGateway(MediaApi.MediaApiClient client)
        {
            _client = client;
        }

        public async Task<GetProductsGalleriesReply> GetProductsGalleries(GetProductsGalleriesRequest request)
        {
            return await _client.GetProductsGalleriesAsync(request);
        }
    }
}