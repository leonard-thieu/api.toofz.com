using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    [Collection(MockDatabaseCollection.Name)]
    public class ReplaysControllerTests
    {
        public ReplaysControllerTests(MockDatabaseFixture fixture)
        {
            mockDb = fixture.CreateMockLeaderboardsContext();
            controller = new ReplaysController(mockDb.Object);
        }

        private readonly Mock<ILeaderboardsContext> mockDb;
        private readonly ReplaysController controller;

        public class Constructor
        {
            [DisplayFact]
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

        public class GetReplaysMethod : ReplaysControllerTests
        {
            public GetReplaysMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly ReplaysPagination pagination = new ReplaysPagination();

            [DisplayFact]
            public async Task ReturnsReplays()
            {
                // Arrange -> Act
                var result = await controller.GetReplays(pagination);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ReplaysEnvelope>>(result);
            }
        }

        public class DisposeMethod : ReplaysControllerTests
        {
            public DisposeMethod(MockDatabaseFixture fixture) : base(fixture) { }

            [DisplayFact]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [DisplayFact]
            public void DisposesMoreThanOnce_OnlyDisposesDbOnce()
            {
                // Arrange -> Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            public IntegrationTests(IntegrationTestsFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [DisplayFact]
            public async Task GetReplays()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/replays?version=75");

                // Assert
                await RespondsWithAsync(response, Resources.GetReplays);
            }
        }
    }
}
