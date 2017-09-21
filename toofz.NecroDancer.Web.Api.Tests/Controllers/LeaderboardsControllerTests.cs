using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class LeaderboardsControllerTests
    {
        [TestClass]
        public class GetLeaderboards
        {
            [TestMethod]
            public async Task ReturnsOkWithLeaderboards()
            {
                // Arrange
                var mockLeaderboards = new MockDbSet<Leaderboard>();
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb
                    .Setup(db => db.Leaderboards)
                    .Returns(mockLeaderboards.Object);
                var categories = new Categories();
                var headers = new LeaderboardHeaders();
                var controller = new LeaderboardsController(mockDb.Object, categories, headers);
                var products = new Products(new Category());
                var modes = new Modes(new Category());
                var runs = new Runs(new Category());
                var characters = new Characters(new Category());

                // Act
                var result = await controller.GetLeaderboards(products, modes, runs, characters);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LeaderboardsDTO>));
            }
        }

        [TestClass]
        public class GetLeaderboardEntries
        {
            [TestMethod]
            public async Task ReturnsOkWithLeaderboardEntries()
            {
                // Arrange
                var mockLeaderboardSet = new MockDbSet<Leaderboard>(new Leaderboard { LeaderboardId = 741312 });

                var mockEntrySet = new MockDbSet<Entry>();

                var mockReplaySet = new MockDbSet<Replay>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Leaderboards).Returns(mockLeaderboardSet.Object);
                mockRepository.Setup(x => x.Entries).Returns(mockEntrySet.Object);
                mockRepository.Setup(x => x.Replays).Returns(mockReplaySet.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    LeaderboardsResources.ReadLeaderboardCategories(),
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var result = await controller.GetLeaderboardEntries(new LeaderboardsPagination(), 741312);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LeaderboardEntriesDTO>));
            }

            [TestMethod]
            public async Task LbidNotInDb_ReturnsNotFound()
            {
                // Arrange
                var mockLeaderboardSet = new MockDbSet<Leaderboard>(new Leaderboard { LeaderboardId = 22 });

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Leaderboards).Returns(mockLeaderboardSet.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    LeaderboardsResources.ReadLeaderboardCategories(),
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetLeaderboardEntries(new LeaderboardsPagination(), 0);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
            }
        }

        [TestClass]
        public class GetDailyLeaderboards
        {
            [TestMethod]
            public async Task ReturnsOkWithDailyLeaderboards()
            {
                // Arrange
                var mockSetDailyLeaderboard = new MockDbSet<DailyLeaderboard>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.DailyLeaderboards).Returns(mockSetDailyLeaderboard.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    new Categories(),
                    new LeaderboardHeaders());

                var products = new Products(new Category());

                // Act
                var result = await controller.GetDailyLeaderboards(new LeaderboardsPagination(), products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyLeaderboardsDTO>));
            }
        }

        [TestClass]
        public class GetDailyLeaderboardEntries
        {
            [TestMethod]
            public async Task ReturnsOkWithDailyLeaderboardEntries()
            {
                // Arrange
                var mockLeaderboards = new MockDbSet<DailyLeaderboard>(
                    new DailyLeaderboard { LeaderboardId = 1 }
                );
                var mockLeaderboardEntries = new MockDbSet<DailyEntry>();
                var mockReplays = new MockDbSet<Replay>();
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb
                    .Setup(db => db.DailyLeaderboards)
                    .Returns(mockLeaderboards.Object);
                mockDb
                    .Setup(db => db.DailyEntries)
                    .Returns(mockLeaderboardEntries.Object);
                mockDb
                    .Setup(db => db.Replays)
                    .Returns(mockReplays.Object);
                var categories = new Categories
                {
                    {
                        "products",
                        new Category
                        {
                            {  "classic", new CategoryItem { Id = 0 } },
                        }
                    },
                };
                var headers = new LeaderboardHeaders();
                var controller = new LeaderboardsController(mockDb.Object, categories, headers);
                var pagination = new LeaderboardsPagination();
                var lbid = 1;

                // Act
                var result = await controller.GetDailyLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyLeaderboardEntriesDTO>));
            }
        }
    }
}
