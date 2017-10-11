using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests
{
    static class Util
    {
        public static ModelMetadata CreateModelMetadata<T>()
        {
            var provider = new EmptyModelMetadataProvider();

            return provider.GetMetadataForType(null, typeof(T));
        }

        public static ILeaderboardsContext CreateLeaderboardsContext()
        {
            var mockDb = new Mock<ILeaderboardsContext>();

            var products = LeaderboardCategories.Products;
            var mockDbProducts = new MockDbSet<Product>(products);
            mockDb.Setup(d => d.Products).Returns(mockDbProducts.Object);

            var modes = LeaderboardCategories.Modes;
            var mockDbModes = new MockDbSet<Mode>(modes);
            mockDb.Setup(d => d.Modes).Returns(mockDbModes.Object);

            var runs = LeaderboardCategories.Runs;
            var mockDbRuns = new MockDbSet<Run>(runs);
            mockDb.Setup(d => d.Runs).Returns(mockDbRuns.Object);

            var characters = LeaderboardCategories.Characters;
            var mockDbCharacters = new MockDbSet<Character>(characters);
            mockDb.Setup(d => d.Characters).Returns(mockDbCharacters.Object);

            return mockDb.Object;
        }
    }
}
