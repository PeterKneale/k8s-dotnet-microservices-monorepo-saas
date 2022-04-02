using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Media.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class GalleryUploadTests
    {
        private readonly ITestOutputHelper _output;
        private readonly MediaApi.MediaApiClient _client;

        public GalleryUploadTests(Fixture api, ITestOutputHelper output)
        {
            _output = output;
            api.OutputHelper = output;
            _client = api.GetClient();
        }

        [Fact]
        public async Task Can_upload_using_pre_signed_url()
        {
            // arrange
            var url = await GetPreSignedUrlUrl();
            
            // act
            var statusCode = await HttpHelper.UploadFile(new Uri(url));
            
            // assert
            statusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Cant_upload_using_url_without_signature()
        {
            // act
            var url = await GetPreSignedUrlUrl();
            var unsignedUrl = url.Substring(0, url.IndexOf("?"));// strip the signature off
            
            // act
            var statusCode = await HttpHelper.UploadFile(new Uri(unsignedUrl));

            // assert
            statusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        
        private async Task<string> GetPreSignedUrlUrl()
        {
            var productId = Guid.NewGuid().ToString();
            var pictureId = Guid.NewGuid().ToString();

            var reply = await _client.GetPreSignedUrlAsync(new GetPreSignedUrlRequest
            {
                ProductId = productId,
                PictureId = pictureId
            });
            var url = reply.Url;

            _output.WriteLine("Pre-signed url can be tested with:");
            _output.WriteLine($"curl -v --upload-file product.png {url}");

            return url;
        }
    }
}