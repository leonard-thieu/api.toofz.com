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
    class ReplaysControllerTests
    {
        [TestClass]
        public class GetReplays
        {
            [TestMethod]
            public async Task ReturnsOkWithReplays()
            {
                // Arrange
                var mockSet = new MockDbSet<Leaderboards.Replay>();

                var mockRepository = new Mock<LeaderboardsContext>();
                mockRepository
                    .Setup(x => x.Replays)
                    .Returns(mockSet.Object);

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new ReplaysController(mockRepository.Object, mockILeaderboardsStoreClient.Object);

                // Act
                var result = await controller.GetReplays(new ReplaysPagination());

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Api.Models.Replays>));
            }
        }

        [TestClass]
        public class PostReplays
        {
            [TestMethod]
            public async Task ReturnsOkWithBulkStore()
            {
                // Arrange
                var mockRepository = new Mock<LeaderboardsContext>();

                var mockILeaderboardsStoreClient = new Mock<ILeaderboardsStoreClient>();

                var controller = new ReplaysController(mockRepository.Object, mockILeaderboardsStoreClient.Object);

                // Act
                var result = await controller.PostReplays(new List<ReplayModel>());

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<BulkStore>));
            }
        }
    }
}
