using Xunit;

namespace Catalog.FunctionalTests
{
    [CollectionDefinition(nameof(Fixture))]
    public class CollectionFixture : ICollectionFixture<Fixture>
    {
        
    }
}