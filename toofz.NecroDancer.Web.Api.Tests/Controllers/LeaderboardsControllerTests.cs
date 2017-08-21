using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Leaderboards.EntityFramework;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class LeaderboardsControllerTests
    {
        [TestClass]
        public class GetLeaderboards
        {
            [TestMethod]
            public async Task ReturnsOkWithLeaderboards()
            {
                // Arrange
                var mockDb = new Mock<LeaderboardsContext>();
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
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Api.Models.Leaderboards>));
            }
        }

        [TestClass]
        public class GetLeaderboardEntries
        {
            [TestMethod]
            public async Task ReturnsOk()
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
                    LeaderboardsResources.ReadLeaderboardCategories(),
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetLeaderboardEntries(new LeaderboardsPagination(), 741312);
                var contentResult = actionResult as OkNegotiatedContentResult<LeaderboardEntries>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsInstanceOfType(contentResult, typeof(OkNegotiatedContentResult<LeaderboardEntries>));
            }

            [TestMethod]
            public async Task ReturnsNotFound()
            {
                // Arrange
                var mockLeaderboardSet = MockHelper.MockSet(new Leaderboards.Leaderboard { LeaderboardId = 22 });

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
        public class GetDailies
        {
            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange
                var mockSetDailyLeaderboard = MockHelper.MockSet<Leaderboards.DailyLeaderboard>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.DailyLeaderboards).Returns(mockSetDailyLeaderboard.Object);

                var controller = new LeaderboardsController(
                    mockRepository.Object,
                    new Categories(),
                    new LeaderboardHeaders());

                var products = new Products(new Category());

                // Act
                var actionResult = await controller.GetDailies(new LeaderboardsPagination(), products);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<DailyLeaderboards>));
            }
        }
    }
}
