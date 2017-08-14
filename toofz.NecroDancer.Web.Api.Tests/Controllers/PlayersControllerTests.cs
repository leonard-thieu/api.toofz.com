using System.Collections.Generic;
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
    public class PlayersControllerTests
    {
        [TestClass]
        public class GetPlayers
        {
            [TestMethod]
            public async Task NoParams_ReturnsPlayers()
            {
                // Arrange
                var mockSet = MockHelper.MockSet<Leaderboards.Player>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Players).Returns(mockSet.Object);

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new PlayersController(
                    mockRepository.Object,
                    mockILeaderboardsStoreClient.Object,
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetPlayers("", new PlayersPagination());
                var contentResult = actionResult as OkNegotiatedContentResult<Players>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }
        }

        [TestClass]
        public class GetPlayer
        {
            [TestMethod]
            public async Task ValidParams_ReturnsPlayerEntries()
            {
                // Arrange
                var mockSetPlayer = MockHelper.MockSet(new Leaderboards.Player { SteamId = 76561197960481221 });
                var mockSetEntry = MockHelper.MockSet<Leaderboards.Entry>();
                var mockSetReplay = MockHelper.MockSet<Leaderboards.Replay>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Players).Returns(mockSetPlayer.Object);
                mockRepository.Setup(x => x.Entries).Returns(mockSetEntry.Object);
                mockRepository.Setup(x => x.Replays).Returns(mockSetReplay.Object);

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new PlayersController(
                    mockRepository.Object,
                    mockILeaderboardsStoreClient.Object,
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetPlayer(76561197960481221);
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayerEntries>));
                var contentResult = actionResult as OkNegotiatedContentResult<PlayerEntries>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }
        }

        [TestClass]
        public class PostPlayers
        {
            [TestMethod]
            public async Task ValidParams_ReturnsBulkStoreDTO()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new PlayersController(
                    mockRepository.Object,
                    mockILeaderboardsStoreClient.Object,
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.PostPlayers(new List<PlayerModel>());
                var contentResult = actionResult as OkNegotiatedContentResult<BulkStore>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
            }

            [TestMethod]
            public async Task InvalidState_ReturnsBadRequest()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new PlayersController(
                    mockRepository.Object,
                    mockILeaderboardsStoreClient.Object,
                    LeaderboardsResources.ReadLeaderboardHeaders());
                controller.ModelState.AddModelError("fakeError", "fakeError");

                // Act
                var actionResult = await controller.PostPlayers(new List<PlayerModel>());

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(InvalidModelStateResult));
            }
        }
    }
}
