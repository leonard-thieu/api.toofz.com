using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using toofz.Data;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MockDatabaseFixture
    {
        public static DbContextOptions<NecroDancerContext> GetNecroDancerContextOptions()
        {
            return new DbContextOptionsBuilder<NecroDancerContext>()
                .UseInMemoryDatabase(databaseName: Constants.NecroDancerContextName)
                .Options;
        }

        public MockDatabaseFixture()
        {
            using (var context = CreateNecroDancerContext())
            {
                context.EnsureSeedData();
            }
        }

        public readonly IEnumerable<Product> Products = JsonConvert.DeserializeObject<IEnumerable<Product>>(LeaderboardsResources.Products);
        public readonly IEnumerable<Mode> Modes = JsonConvert.DeserializeObject<IEnumerable<Mode>>(LeaderboardsResources.Modes);
        public readonly IEnumerable<Run> Runs = JsonConvert.DeserializeObject<IEnumerable<Run>>(LeaderboardsResources.Runs);
        public readonly IEnumerable<Character> Characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(LeaderboardsResources.Characters);

        public NecroDancerContext CreateNecroDancerContext()
        {
            return new NecroDancerContext(GetNecroDancerContextOptions());
        }
    }

    [CollectionDefinition(Name)]
    public class MockDatabaseCollection : ICollectionFixture<MockDatabaseFixture>
    {
        public const string Name = "Mock database collection";
    }
}
