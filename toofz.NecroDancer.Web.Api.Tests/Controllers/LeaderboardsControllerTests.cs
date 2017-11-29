using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using Ninject;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class LeaderboardsControllerTests
    {
        private static IEnumerable<Leaderboard> Leaderboards
        {
            get
            {
                return new[]
                {
                    new Leaderboard
                    {
                        Product = new Product(1, "amplified", "Amplified"),
                        Mode = new Mode(0, "standard", "Standard"),
                        Run = new Run(1, "speed", "Speed"),
                        Character = new Character(0, "cadence", "Cadence"),
                    },
                };
            }
        }

        public class Constructor
        {
            [Fact]
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

        public class GetLeaderboardsMethod
        {
            [Fact]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var dbLeaderboards = new FakeDbSet<Leaderboard>();
                mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);
                var controller = new LeaderboardsController(db);
                var products = new Products(LeaderboardCategories.Products.Select(p => p.Name).ToList());
                var modes = new Modes(LeaderboardCategories.Modes.Select(m => m.Name).ToList());
                var runs = new Runs(LeaderboardCategories.Runs.Select(r => r.Name).ToList());
                var characters = new Characters(LeaderboardCategories.Characters.Select(c => c.Name).ToList());

                // Act
                var result = await controller.GetLeaderboards(products, modes, runs, characters);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<LeaderboardsEnvelope>>(result);
            }
        }

        public class GetLeaderboardEntriesMethod
        {
            [Fact]
            public async Task LeaderboardNotFound_ReturnsNotFound()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var leaderboards = new List<Leaderboard>
                {
                    new Leaderboard { LeaderboardId = 22 },
                };
                var dbLeaderboards = new FakeDbSet<Leaderboard>(leaderboards);
                mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();

                // Act
                var actionResult = await controller.GetLeaderboardEntries(pagination, 0);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(actionResult);
            }

            [Fact]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var leaderboards = new List<Leaderboard>
                {
                    new Leaderboard
                    {
                        LeaderboardId = 741312,
                        Product = new Product(1, "myName", "myDisplayName"),
                        Mode = new Mode(1, "myName", "myDisplayName"),
                        Run = new Run(1, "myName", "myDisplayName"),
                        Character = new Character(1, "myName", "myDisplayName"),
                    },
                };
                var dbLeaderboards = new FakeDbSet<Leaderboard>(leaderboards);
                mockDb.Setup(x => x.Leaderboards).Returns(dbLeaderboards);
                var dbEntries = new FakeDbSet<Entry>();
                mockDb.Setup(x => x.Entries).Returns(dbEntries);
                var dbReplays = new FakeDbSet<Replay>();
                mockDb.Setup(x => x.Replays).Returns(dbReplays);
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();

                // Act
                var result = await controller.GetLeaderboardEntries(pagination, 741312);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<LeaderboardEntriesDTO>>(result);
            }
        }

        public class GetDailyLeaderboardsMethod
        {
            [Fact]
            public async Task ReturnsDailyLeaderboards()
            {
                // Arrange
                var db = Util.CreateLeaderboardsContext();
                var mockDb = Mock.Get(db);
                var dbDailyLeaderboards = new FakeDbSet<DailyLeaderboard>();
                mockDb.Setup(x => x.DailyLeaderboards).Returns(dbDailyLeaderboards);
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();
                var productsParams = new Products(LeaderboardCategories.Products.Select(p => p.Name).ToList());

                // Act
                var result = await controller.GetDailyLeaderboards(pagination, productsParams);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyLeaderboardsEnvelope>>(result);
            }
        }

        public class GetDailyLeaderboardEntriesMethod
        {
            [Fact]
            public async Task DailyLeaderboardNotFound_ReturnsNotFound()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var dbDailyLeaderboards = new FakeDbSet<DailyLeaderboard>();
                mockDb.Setup(d => d.DailyLeaderboards).Returns(dbDailyLeaderboards);
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();

                // Act
                var actionResult = await controller.GetDailyLeaderboardEntries(pagination, 0);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(actionResult);
            }

            [Fact]
            public async Task ReturnsDailyLeaderboardEntries()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var dailyLeaderboards = new List<DailyLeaderboard>
                {
                    new DailyLeaderboard
                    {
                        LeaderboardId = 1,
                        Product = new Product(1, "myName", "myDisplayName"),
                    },
                };
                var dbDailyLeaderboards = new FakeDbSet<DailyLeaderboard>(dailyLeaderboards);
                mockDb.Setup(d => d.DailyLeaderboards).Returns(dbDailyLeaderboards);
                var dbDailyEntries = new FakeDbSet<DailyEntry>();
                mockDb.Setup(d => d.DailyEntries).Returns(dbDailyEntries);
                var dbReplays = new FakeDbSet<Replay>();
                mockDb.Setup(d => d.Replays).Returns(dbReplays);
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();
                var lbid = 1;

                // Act
                var result = await controller.GetDailyLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyLeaderboardEntriesDTO>>(result);
            }
        }

        public class DisposeMethod
        {
            [Fact]
            public void DisposesDb()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);

                // Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [Fact]
            public void DisposesMoreThanOnce_OnlyDisposesDbOnce()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);

                // Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            [Fact]
            public async Task GetLeaderboards()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var dbLeaderboards = new FakeDbSet<Leaderboard>(Leaderboards);
                mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);
                Kernel.Rebind<ILeaderboardsContext>().ToConstant(db);

                // Act
                var response = await Server.HttpClient.GetAsync("/leaderboards");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(Resources.GetLeaderboards, content, ignoreLineEndingDifferences: true);
            }
        }
    }
}
