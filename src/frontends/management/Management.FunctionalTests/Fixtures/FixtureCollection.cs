using Xunit;

namespace Management.FunctionalTests.Fixtures
{
    [CollectionDefinition(nameof(Fixture))]
    public class FixtureCollection : ICollectionFixture<Fixture> { }
}