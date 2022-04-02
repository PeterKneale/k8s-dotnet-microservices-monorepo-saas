using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Media.Application;
using Media.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Xunit;
using Xunit.Abstractions;

namespace Media.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class BucketTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IMinioClientFactory _factory;

        public BucketTests(Fixture api, ITestOutputHelper output)
        {
            _output = output;
            api.OutputHelper = output;
            _factory = api.Services.GetRequiredService<IMinioClientFactory>();
        }

        [Fact]
        public async Task Get_bucket_policy()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var policy = await client.GetPolicyAsync(new GetPolicyArgs().WithBucket(Buckets.Media));
            _output.WriteLine(policy);

            // assert
            policy.Should().Contain("\"Action\":[\"s3:GetObject\"]", "only get permissions");
            policy.Should().Contain("\"Resource\":[\"arn:aws:s3:::media/*\"]", "only media bucket");
        }

        [Fact]
        public async Task Check_default_bucket_exist()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var response = await client.ListBucketsAsync();
            foreach (var bucket in response.Buckets)
            {
                _output.WriteLine($"Found bucket: {bucket.Name}");
            }

            // assert
            response.Buckets.Select(x => x.Name).Should().Contain(Buckets.Media);
        }
    }
}