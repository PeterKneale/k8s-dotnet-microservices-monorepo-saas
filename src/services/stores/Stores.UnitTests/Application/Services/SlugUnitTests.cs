using Stores.Application.Services;
using Xunit;

namespace Stores.UnitTests.Application.Services
{
    public class SlugUnitTests
    {
        [Theory]
        // lowered
        [InlineData("CocaCola", "cocacola")]
        [InlineData("Coca-Cola", "coca-cola")]
        // spaces becomes dashes 
        [InlineData("Coca Cola", "coca-cola")]
        [InlineData("Coca Cola Australia Pty Ltd", "coca-cola-australia-pty-ltd")]
        // invalid characters removed
        [InlineData("Coca#Cola", "cocacola")]
        [InlineData("Coca_Cola", "cocacola")]
        public void Validate_slugs(string name, string expectedSlug) =>
            Assert.Equal(expectedSlug, name.GenerateSlug());
    }
}