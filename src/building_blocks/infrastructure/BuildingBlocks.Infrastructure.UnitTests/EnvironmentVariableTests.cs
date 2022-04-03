using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Infrastructure.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace BuildingBlocks.Infrastructure.UnitTests
{
    public class EnvironmentVariableTests
    {
        private readonly IConfigurationRoot _configuration;
    
        public EnvironmentVariableTests(ITestOutputHelper output)
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(GetEnvironmentVariables())
                .Build();
            foreach (var item in _configuration.AsEnumerable())
            {
                output.WriteLine($"{item.Key}={item.Value}");
            }
        }
    
        [Fact] 
        public void GetRabbitUri() => 
            _configuration.GetRabbitUri().Should().Be("amqp://admin:password@rabbit_host:1234/");
        
        [Fact] 
        public void GetElasticSearchHost() => 
            _configuration.GetElasticSearchUri().Should().Be("http://elastic_host:5678/");
        
        [Fact] 
        public void GetCartsServiceGrpcUri() => 
            _configuration.GetCartsServiceGrpcUri().Should().Be("http://localhost:5001/");
        
        [Fact] 
        public void GetRegistrationBackendGrpcUri() => 
            _configuration.GetRegistrationBackendGrpcUri().Should().Be("http://localhost:5001/");

        private static Dictionary<string, string> GetEnvironmentVariables() => 
            GetK8SEnvVars()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split('=', StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(kv => kv[0].Trim(), kv => kv[1].Trim());

        private static string GetK8SEnvVars() => @"
            SAAS_INFRA_RABBITMQ_HOST=rabbit_host
            SAAS_INFRA_RABBITMQ_PORT=1234
            SAAS_INFRA_ELASTICSEARCH_HOST=elastic_host
            SAAS_INFRA_ELASTICSEARCH_PORT=5678";
    }
}