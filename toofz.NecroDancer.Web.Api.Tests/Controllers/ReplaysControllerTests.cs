using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;

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

                // Act
                var controller = new ReplaysController(db);

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
                var mockDb = new Mock<ILeaderboardsContext>();
                var db = mockDb.Object;
                var replays = new List<Replay>();
                var dbReplays = new FakeDbSet<Replay>(replays);
                mockDb.Setup(x => x.Replays).Returns(dbReplays);
                var controller = new ReplaysController(db);
                var pagination = new ReplaysPagination();

                // Act
                var result = await controller.GetReplays(pagination);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ReplaysEnvelope>>(result);
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
                var controller = new ReplaysController(db);

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
                var controller = new ReplaysController(db);

                // Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            public IntegrationTests(IntegrationTestsFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task GetReplaysMethod()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/replays?version=75");

                // Assert
                await RespondsWithAsync(response, Resources.GetReplays);
            }
        }
    }
}
