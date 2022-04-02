using Xunit;

namespace Stores.FunctionalTests
{
    [CollectionDefinition(nameof(Fixture))]
    public class CollectionFixture : ICollectionFixture<Fixture>
    {
        
    }
}