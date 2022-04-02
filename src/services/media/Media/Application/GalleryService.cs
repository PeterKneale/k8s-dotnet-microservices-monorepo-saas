using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Google.Protobuf.Collections;
using Media.Infrastructure;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;

namespace Media.Application
{
    public interface IGalleryService
    {
        Gallery GetGallery(string productId);
    }

    public class GalleryService : IGalleryService
    {
        private readonly ILogger<GalleryService> _log;
        private readonly MinioClient _client;

        public GalleryService(IMinioClientFactory factory, ILogger<GalleryService> log)
        {
            _log = log;
            _client = factory.CreateClient();
        }

        public Gallery GetGallery(string productId)
        {
            var prefix = Folders.GetProductGalleryFolder(productId);
            var bucket = Buckets.Media;

            _log.LogInformation($"Getting gallery from bucket {bucket} with prefix {prefix}");

            try
            {
                var urls = GetUrls(bucket, prefix);
                return new Gallery {PictureUrls = {urls}};
            }
            catch (MinioException e)
            {
                if (e.ServerMessage.Contains("is empty"))
                {
                    return new Gallery {PictureUrls = {Enumerable.Empty<string>()}};
                }
                _log.LogError(e, "unable to get gallery");
                throw;
            }
        }

        private IEnumerable<string> GetUrls(string bucket, string prefix)
        {
            var observable = _client.ListObjectsAsync(new ListObjectsArgs()
                .WithBucket(bucket)
                .WithRecursive(true)
                .WithPrefix(prefix));

            var urls = new List<string>();
            using var subscription = observable.Subscribe(
                item => urls.Add($"{bucket}/{item.Key}"),
                ex => throw ex
            );

            observable.Wait();
            subscription.Dispose();

            return urls;
        }
    }
}