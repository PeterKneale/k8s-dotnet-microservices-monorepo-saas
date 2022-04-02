using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Media.Infrastructure;
using Microsoft.Extensions.Logging;
using Minio;

namespace Media.Application
{
    public interface IGalleryUploadService
    {
        Task<Uri> GenerateForProduct(string productId, string pictureId);
    }

    public class GalleryUploadService : IGalleryUploadService
    {
        private readonly ILogger<GalleryService> _log;
        private readonly MinioClient _client;

        public GalleryUploadService(IMinioClientFactory factory, ILogger<GalleryService> log)
        {
            _log = log;
            _client = factory.CreateClient();
        }

        public async Task<Uri> GenerateForProduct(string productId, string pictureId)
        {
            var file = Folders.GetPictureFile(productId, pictureId);

            _log.LogInformation($"Generating presigned url for {file}");

            var url = await _client
                .PresignedPutObjectAsync(new PresignedPutObjectArgs()
                    .WithBucket(Buckets.Media)
                    .WithHeaders(new Dictionary<string, string> {{"Content-Type", "multipart/form-data"}})
                    .WithExpiry(3600)
                    .WithObject(file));
            return new Uri(url);
        }
    }

}