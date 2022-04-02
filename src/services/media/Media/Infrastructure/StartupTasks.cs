using System.Threading.Tasks;
using Media.Application;
using Microsoft.Extensions.Logging;
using Minio;

namespace Media.Infrastructure
{
    public interface IMinioSetup
    {
        Task CreateBuckets();
    }

    /// <summary>
    /// https://docs.min.io/docs/dotnet-client-quickstart-guide.html
    /// console: http://localhost:9001/
    /// </summary>
    public class MinioSetup : IMinioSetup
    {
        private readonly ILogger<MinioSetup> _logs;
        private readonly MinioClient _client;

        public MinioSetup(IMinioClientFactory factory, ILogger<MinioSetup> logs)
        {
            _logs = logs;
            _client = factory.CreateClient();
        }

        public async Task CreateBuckets()
        {
            await EnsureBucketExists(Buckets.Media);
            await EnsureBucketPolicy(Buckets.Media, Buckets.MediaPolicy); 
        }
        
        private async Task EnsureBucketPolicy(string bucket, string policy)
        {
            _logs.LogInformation($"Setting policy on bucket '{bucket}' to '{policy}'");
            await _client.SetPolicyAsync(new SetPolicyArgs().WithBucket(bucket).WithPolicy(policy));
        }

        private async Task EnsureBucketExists(string bucket)
        {
            _logs.LogInformation($"Ensuring bucket '{bucket}' exists");
            var found = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
            if (!found)
            {
                await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
            }
        }
    }
}