using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using Ninject;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using toofz.TestsShared;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class ReplaysControllerTests
    {
        private static IEnumerable<Replay> Replays
        {
            get
            {
                return new[]
                {
                    new Replay { ReplayId = 25094445621522262 },
                    new Replay { ReplayId = 25094445622197065 },
                    new Replay { ReplayId = 25094445622344966 },
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
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();

                // Act
                var controller = new ReplaysController(db, storeClient);

                // Assert
                Assert.IsAssignableFrom<ReplaysController>(controller);
            }
        }

        public class GetReplaysMethod
        {
            [Fact]
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
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ReplaysEnvelope>>(result);
            }
        }

        public class PostReplaysMethod
        {
            [Fact]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                var replays = new List<ReplayModel>
                {
                    new ReplayModel
                    {
                        ReplayId = 42385384753,
                        ErrorCode = null,
                        Seed = 3548,
                        Version = 75,
                        KilledBy = "BOMB",
                    },
                };
                var db = Mock.Of<ILeaderboardsContext>();
                var mockStoreClient = new Mock<ILeaderboardsStoreClient>();
                mockStoreClient.Setup(s => s.BulkUpsertAsync(It.IsAny<IEnumerable<Replay>>(), default)).Returns(Task.FromResult(replays.Count));
                var storeClient = mockStoreClient.Object;
                var controller = new ReplaysController(db, storeClient);

                // Act
                var actionResult = await controller.PostReplays(replays);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<BulkStoreDTO>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<BulkStoreDTO>)actionResult;
                var content = contentResult.Content;
                Assert.Equal(1, content.RowsAffected);
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
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new ReplaysController(db, storeClient);

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
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new ReplaysController(db, storeClient);

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
            public async Task GetReplaysMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var replays = Replays;
                var mockDbReplays = new MockDbSet<Replay>(replays);
                var dbReplays = mockDbReplays.Object;
                mockDb.Setup(d => d.Replays).Returns(dbReplays);

                // Act
                var response = await Server.HttpClient.GetAsync("/replays");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(Resources.GetReplays, content, ignoreLineEndingDifferences: true);
            }
        }
    }
}
