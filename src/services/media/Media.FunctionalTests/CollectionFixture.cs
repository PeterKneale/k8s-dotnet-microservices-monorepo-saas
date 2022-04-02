using Xunit;

namespace Media.FunctionalTests
{
    [CollectionDefinition(nameof(Fixture))]
    public class CollectionFixture : ICollectionFixture<Fixture>
    {
        
    }
}