using Xunit;

namespace Registration.FunctionalTests
{
    [CollectionDefinition(nameof(Fixture))]
    public class CollectionFixture : ICollectionFixture<Fixture>
    {
        
    }
}