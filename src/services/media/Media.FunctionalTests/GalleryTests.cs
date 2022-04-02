using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Media.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class GalleryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly MediaApi.MediaApiClient _client;
        private readonly Uri _minioUri;

        public GalleryTests(Fixture api, ITestOutputHelper output)
        {
            _minioUri = api.MinioUri;
            _output = output;
            api.OutputHelper = output;
            _client = api.GetClient();
        }

        [Fact]
        public async Task Can_get_product_gallery()
        {
            // arrange
            var productId = Guid.NewGuid().ToString();
            var pictureId1 = Guid.NewGuid().ToString();
            var pictureId2 = Guid.NewGuid().ToString();
            var pictureId3 = Guid.NewGuid().ToString();

            // act
            await UploadProductPicture(productId, pictureId1);
            await UploadProductPicture(productId, pictureId2);
            await UploadProductPicture(productId, pictureId3);
            var result = _client.GetProductGallery(new GetProductGalleryRequest {ProductId = productId});

            // assert
            result.Gallery.PictureUrls.Should().HaveCount(3);
            foreach (var picture in result.Gallery.PictureUrls)
            {
                await DownloadProductPicture(picture);
            }
        }

        [Fact]
        public void Can_get_empty_product_gallery()
        {
            // arrange
            var productId = Guid.NewGuid().ToString();

            // act
            var result = _client.GetProductGallery(new GetProductGalleryRequest {ProductId = productId});

            // assert
            result.Gallery.PictureUrls.Should().BeEmpty();
        }
        
        private async Task UploadProductPicture(string productId, string pictureId)
        {
            var reply = await _client.GetPreSignedUrlAsync(new GetPreSignedUrlRequest
            {
                ProductId = productId,
                PictureId = pictureId
            });
            var uri = new Uri(reply.Url);
            
            _output.WriteLine($"Uploading {uri}");
            
            var result = await HttpHelper.UploadFile(uri);
            result.Should().Be(HttpStatusCode.OK);
        }
        
        private async Task DownloadProductPicture(string url)
        {
            var uri = new Uri(_minioUri, new Uri(url, UriKind.Relative));
            
            _output.WriteLine($"Downloading {uri}");
            
            using var client = new HttpClient();
            var response = await client.GetAsync(uri);
            _output.WriteLine(JsonConvert.SerializeObject(response));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}