using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards.EntityFramework;
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
                var mockLeaderboards = MockHelper.MockSet<Leaderboards.Leaderboard>();
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb
                    .Setup(db => db.Leaderboards)
                    .Returns(mockLeaderboards.Object);
                var categories = new Leaderboards.Categories();
                var headers = new Leaderboards.LeaderboardHeaders();
                var controller = new LeaderboardsController(mockDb.Object, categories, headers);
                var products = new Products(new Leaderboards.Category());
                var modes = new Modes(new Leaderboards.Category());
                var runs = new Runs(new Leaderboards.Category());
                var characters = new Characters(new Leaderboards.Category());

                // Act
                var result = await controller.GetLeaderboards(products, modes, runs, characters);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Api.Models.Leaderboards>));
            }
        }

        [TestClass]
        public class GetLeaderboardEntries
        {
            [TestMethod]
            public async Task ReturnsOkWithLeaderboardEntries()
            {
                // Arrange
                var mockLeaderboardSet = MockHelper.MockSet(new Leaderboards.Leaderboard { LeaderboardId = 741312 });

                var mockEntrySet = MockHelper.MockSet<Leaderboards.Entry>();

                var mockReplaySet = MockHelper.MockSet<Leaderboards.Replay>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Leaderboards).Returns(mockLeaderboardSet.Object);
                mockRepository.Setup(x => x.Entries).Returns(mockEntrySet.Object);
                mockRepository.Setup(x => x.Replays).Returns(mockReplaySet.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    Leaderboards.LeaderboardsResources.ReadLeaderboardCategories(),
                    Leaderboards.LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var result = await controller.GetLeaderboardEntries(new LeaderboardsPagination(), 741312);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LeaderboardEntries>));
            }

            [TestMethod]
            public async Task LbidNotInDb_ReturnsNotFound()
            {
                // Arrange
                var mockLeaderboardSet = MockHelper.MockSet(new Leaderboards.Leaderboard { LeaderboardId = 22 });

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Leaderboards).Returns(mockLeaderboardSet.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    Leaderboards.LeaderboardsResources.ReadLeaderboardCategories(),
                    Leaderboards.LeaderboardsResources.ReadLeaderboardHeaders());

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
                var mockSetDailyLeaderboard = MockHelper.MockSet<Leaderboards.DailyLeaderboard>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.DailyLeaderboards).Returns(mockSetDailyLeaderboard.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    new Leaderboards.Categories(),
                    new Leaderboards.LeaderboardHeaders());

                var products = new Products(new Leaderboards.Category());

                // Act
                var result = await controller.GetDailyLeaderboards(new LeaderboardsPagination(), products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyLeaderboards>));
            }
        }

        [TestClass]
        public class GetDailyLeaderboardEntries
        {
            [TestMethod]
            public async Task ReturnsOkWithDailyLeaderboardEntries()
            {
                // Arrange
                var mockLeaderboards = MockHelper.MockSet(
                    new Leaderboards.DailyLeaderboard { LeaderboardId = 1 }
                );
                var mockLeaderboardEntries = MockHelper.MockSet<Leaderboards.DailyEntry>();
                var mockReplays = MockHelper.MockSet<Leaderboards.Replay>();
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
                var categories = new Leaderboards.Categories
                {
                    {
                        "products",
                        new Leaderboards.Category
                        {
                            {  "classic", new Leaderboards.CategoryItem { id = 0 } },
                        }
                    },
                };
                var headers = new Leaderboards.LeaderboardHeaders();
                var controller = new LeaderboardsController(mockDb.Object, categories, headers);
                var pagination = new LeaderboardsPagination();
                var lbid = 1;

                // Act
                var result = await controller.GetDailyLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyLeaderboardEntries>));
            }
        }
    }
}
