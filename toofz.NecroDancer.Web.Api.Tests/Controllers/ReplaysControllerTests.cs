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
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var db = Mock.Of<ILeaderboardsContext>();
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();

                // Act
                var controller = new ReplaysController(db, storeClient);

                // Assert
                Assert.IsInstanceOfType(controller, typeof(ReplaysController));
            }
        }

        [TestClass]
        public class GetReplaysMethod
        {
            [TestMethod]
            public async Task ReturnsReplays()
            {
                // Arrange
                var mockReplays = new MockDbSet<Replay>();
                var replays = mockReplays.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new ReplaysController(db, storeClient);
                var pagination = new ReplaysPagination();

                // Act
                var result = await controller.GetReplays(pagination);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ReplaysEnvelope>));
            }
        }

        [TestClass]
        public class PostReplaysMethod
        {
            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                var db = Mock.Of<ILeaderboardsContext>();
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new ReplaysController(db, storeClient);
                var replays = new[] { new ReplayModel() };

                // Act
                var result = await controller.PostReplays(replays);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<BulkStoreDTO>));
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void DisposesDb()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new ReplaysController(db, storeClient);

                // Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [TestMethod]
            public void DisposesMoreThanOnce_OnlyDisposesDbOnce()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new ReplaysController(db, storeClient);

                // Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }
    }
}
