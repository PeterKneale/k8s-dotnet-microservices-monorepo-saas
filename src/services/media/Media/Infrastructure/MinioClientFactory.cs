using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Minio;

namespace Media.Infrastructure
{
    public interface IMinioClientFactory
    {
        MinioClient CreateClient();
    }
    
    public class MinioClientFactory : IMinioClientFactory
    {
        private readonly IConfiguration _configuration;
        
        public MinioClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public MinioClient CreateClient()
        {
            var minioClient = new MinioClient()
                .WithEndpoint(_configuration.GetMinioHost(), _configuration.GetMinioPort())
                .WithCredentials(_configuration.GetMinioAccessKey(), _configuration.GetMinioSecretKey())
                .Build();
            
            var app = GetType().Assembly.GetName();
            minioClient.SetAppInfo(app.Name, app.Version?.ToString() ?? "unknown");
            if (_configuration.GetMinioTraceEnabled())
            {
                minioClient.SetTraceOn();    
            }
            return minioClient;
        }
    }
}