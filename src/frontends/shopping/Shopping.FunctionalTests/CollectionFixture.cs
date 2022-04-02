using Xunit;

namespace Shopping.FunctionalTests
{
    [CollectionDefinition(nameof(Fixture))]
    public class CollectionFixture : ICollectionFixture<Fixture>
    {
        
    }
}