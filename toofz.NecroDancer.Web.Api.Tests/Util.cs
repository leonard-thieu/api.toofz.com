using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using Moq;
using toofz.NecroDancer.Leaderboards;

namespace toofz.NecroDancer.Web.Api.Tests
{
    internal static class Util
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
            var dbProducts = new FakeDbSet<Product>(products);
            mockDb.Setup(d => d.Products).Returns(dbProducts);

            var modes = LeaderboardCategories.Modes;
            var dbModes = new FakeDbSet<Mode>(modes);
            mockDb.Setup(d => d.Modes).Returns(dbModes);

            var runs = LeaderboardCategories.Runs;
            var dbRuns = new FakeDbSet<Run>(runs);
            mockDb.Setup(d => d.Runs).Returns(dbRuns);

            var characters = LeaderboardCategories.Characters;
            var dbCharacters = new FakeDbSet<Character>(characters);
            mockDb.Setup(d => d.Characters).Returns(dbCharacters);

            return mockDb.Object;
        }
    }
}
