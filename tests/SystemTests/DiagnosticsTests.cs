using SystemTests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SystemTests
{
    public class DiagnosticsTests : TestBase, IClassFixture<Fixture>
    {
        private readonly ITestOutputHelper _output;
        
        public DiagnosticsTests(Fixture container, ITestOutputHelper output) : base(container)
        {
            _output = output;
        }
        
        [Fact]
        public void OutputConfiguration()
        {
            _output.WriteLine($"Environment name is '{ConfigurationHelper.EnvironmentName}'");
        }
    }
}