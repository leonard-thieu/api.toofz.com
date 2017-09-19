using System.Collections.Generic;
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
    class PlayersControllerTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var db = new LeaderboardsContext();
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var leaderboardHeaders = new LeaderboardHeaders();

                // Act
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);

                // Assert
                Assert.IsInstanceOfType(controller, typeof(PlayersController));
            }
        }

        [TestClass]
        public class GetPlayersMethod
        {
            [TestMethod]
            public async Task ReturnsPlayers()
            {
                // Arrange
                var mockSet = new MockDbSet<Leaderboards.Player>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository.Setup(x => x.Players).Returns(mockSet.Object);

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new PlayersController(
                    mockRepository.Object,
                    mockILeaderboardsStoreClient.Object,
                    LeaderboardsResources.ReadLeaderboardHeaders());

                // Act
                var actionResult = await controller.GetPlayers(new PlayersPagination());
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
                var mockSetPlayer = new MockDbSet<Leaderboards.Player>(new Leaderboards.Player { SteamId = 76561197960481221 });
                var mockSetEntry = new MockDbSet<Leaderboards.Entry>();
                var mockSetReplay = new MockDbSet<Leaderboards.Replay>();

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
        }
    }
}
