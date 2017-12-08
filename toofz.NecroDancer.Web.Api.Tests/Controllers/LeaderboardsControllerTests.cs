using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    [Collection(MockDatabaseCollection.Name)]
    public class LeaderboardsControllerTests
    {
        public LeaderboardsControllerTests(MockDatabaseFixture fixture)
        {
            this.fixture = fixture;
            mockDb = fixture.CreateMockLeaderboardsContext();
            controller = new LeaderboardsController(mockDb.Object);
        }

        private readonly MockDatabaseFixture fixture;
        private readonly Mock<ILeaderboardsContext> mockDb;
        private readonly LeaderboardsController controller;

        public class Constructor
        {
            [DisplayFact]
            public void ReturnsInstance()
            {
                // Arrange
                var db = Mock.Of<ILeaderboardsContext>();

                // Act
                var controller = new LeaderboardsController(db);

                // Assert
                Assert.IsAssignableFrom<LeaderboardsController>(controller);
            }
        }

        public class GetLeaderboardsMethod : LeaderboardsControllerTests
        {
            public GetLeaderboardsMethod(MockDatabaseFixture fixture) : base(fixture) { }

            [DisplayFact]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                var products = new Products(fixture.Products.Select(p => p.Name).ToList());
                var modes = new Modes(fixture.Modes.Select(m => m.Name).ToList());
                var runs = new Runs(fixture.Runs.Select(r => r.Name).ToList());
                var characters = new Characters(fixture.Characters.Select(c => c.Name).ToList());

                // Act
                var result = await controller.GetLeaderboards(products, modes, runs, characters);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<LeaderboardsEnvelope>>(result);
            }
        }

        public class GetLeaderboardEntriesMethod : LeaderboardsControllerTests
        {
            public GetLeaderboardEntriesMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly LeaderboardsPagination pagination = new LeaderboardsPagination();

            [DisplayFact]
            public async Task LeaderboardNotFound_ReturnsNotFound()
            {
                // Arrange
                var lbid = 0;

                // Act
                var actionResult = await controller.GetLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(actionResult);
            }

            [DisplayFact]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                var lbid = 741312;

                // Act
                var result = await controller.GetLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<LeaderboardEntriesDTO>>(result);
            }
        }

        public class GetDailyLeaderboardsMethod : LeaderboardsControllerTests
        {
            public GetDailyLeaderboardsMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly LeaderboardsPagination pagination = new LeaderboardsPagination();

            [DisplayFact]
            public async Task ReturnsDailyLeaderboards()
            {
                // Arrange
                var products = new Products(fixture.Products.Select(p => p.Name).ToList());

                // Act
                var result = await controller.GetDailyLeaderboards(pagination, products);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyLeaderboardsEnvelope>>(result);
            }
        }

        public class GetDailyLeaderboardEntriesMethod : LeaderboardsControllerTests
        {
            public GetDailyLeaderboardEntriesMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly LeaderboardsPagination pagination = new LeaderboardsPagination();

            [DisplayFact]
            public async Task DailyLeaderboardNotFound_ReturnsNotFound()
            {
                // Arrange
                var lbid = 0;

                // Act
                var actionResult = await controller.GetDailyLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(actionResult);
            }

            [DisplayFact]
            public async Task ReturnsDailyLeaderboardEntries()
            {
                // Arrange
                var lbid = 391760;

                // Act
                var result = await controller.GetDailyLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyLeaderboardEntriesDTO>>(result);
            }
        }

        public class DisposeMethod : LeaderboardsControllerTests
        {
            public DisposeMethod(MockDatabaseFixture fixture) : base(fixture) { }

            [DisplayFact]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [DisplayFact]
            public void DisposesMoreThanOnce_OnlyDisposesDbOnce()
            {
                // Arrange -> Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            public IntegrationTests(IntegrationTestsFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [DisplayFact]
            public async Task GetLeaderboards()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/leaderboards");

                // Assert
                await RespondsWithAsync(response, Resources.GetLeaderboards);
            }
        }
    }
}
