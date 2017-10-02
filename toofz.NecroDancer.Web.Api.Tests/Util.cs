using System.Globalization;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests
{
    static class Util
    {
        public static ValueProviderResult CreateValueProviderResult(object rawValue)
        {
            return new ValueProviderResult(rawValue, rawValue.ToString(), CultureInfo.InvariantCulture);
        }

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
            var dbProducts = mockDbProducts.Object;
            mockDb.Setup(d => d.Products).Returns(dbProducts);
            var modes = LeaderboardCategories.Modes;
            var mockDbModes = new MockDbSet<Mode>(modes);
            var dbModes = mockDbModes.Object;
            mockDb.Setup(d => d.Modes).Returns(dbModes);
            var runs = LeaderboardCategories.Runs;
            var mockDbRuns = new MockDbSet<Run>(runs);
            var dbRuns = mockDbRuns.Object;
            mockDb.Setup(d => d.Runs).Returns(dbRuns);
            var characters = LeaderboardCategories.Characters;
            var mockDbCharacters = new MockDbSet<Character>(characters);
            var dbCharacters = mockDbCharacters.Object;
            mockDb.Setup(d => d.Characters).Returns(dbCharacters);
            var db = mockDb.Object;

            return db;
        }
    }
}
