using System.Threading.Tasks;
using Media;

namespace Shopping.Application.Services
{
    public interface IMediaServiceGateway
    {
        Task<GetProductsGalleriesReply> GetProductsGalleries(GetProductsGalleriesRequest request);
    }
}