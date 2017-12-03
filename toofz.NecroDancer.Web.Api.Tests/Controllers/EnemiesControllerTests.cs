using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    [Collection(MockDatabaseCollection.Name)]
    public class EnemiesControllerTests
    {
        public EnemiesControllerTests(MockDatabaseFixture fixture)
        {
            mockDb = fixture.CreateMockNecroDancerContext();
            controller = new EnemiesController(mockDb.Object);
        }

        private readonly Mock<INecroDancerContext> mockDb;
        private readonly EnemiesController controller;

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var db = Mock.Of<INecroDancerContext>();

                // Act
                var controller = new EnemiesController(db);

                // Assert
                Assert.IsAssignableFrom<EnemiesController>(controller);
            }
        }

        public class GetEnemiesMethod : EnemiesControllerTests
        {
            public GetEnemiesMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly EnemiesPagination pagination = new EnemiesPagination();

            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EnemiesEnvelope>>(result);
            }

            [Fact]
            public async Task ReturnsEnemies()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.Equal(10, contentEnemies.Count());
            }

            [Fact]
            public async Task ResultsAreOrderedByElemenyNameThenByType()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.Equal(new[]
                {
                    "armadillo",
                    "armadillo",
                    "armadillo",
                    "armoredskeleton",
                    "armoredskeleton",
                    "armoredskeleton",
                    "armoredskeleton",
                    "banshee",
                    "banshee",
                    "bat",
                }, contentEnemies.Select(e => e.Name));
                Assert.Equal(new[]
                {
                    1,
                    2,
                    3,
                    1,
                    2,
                    3,
                    4,
                    1,
                    2,
                    1,
                }, contentEnemies.Select(e => e.Type));
            }

            [Fact]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.Limit = 2;

                // Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.Equal(2, contentEnemies.Count());
            }

            [Fact]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                pagination.Offset = 2;

                // Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                var first = contentEnemies.First();
                Assert.Equal("armadillo", first.Name);
                Assert.Equal(3, first.Type);
            }
        }

        public class GetEnemiesByAttributeMethod : EnemiesControllerTests
        {
            public GetEnemiesByAttributeMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly EnemiesPagination pagination = new EnemiesPagination();

            [Theory]
            [InlineData("boss")]
            [InlineData("bounce-on-movement-fail")]
            [InlineData("floating")]
            [InlineData("ignore-liquids")]
            [InlineData("ignore-walls")]
            [InlineData("is-monkey-like")]
            [InlineData("massive")]
            [InlineData("miniboss")]
            public async Task ReturnsOk(string attribute)
            {
                // Arrange -> Act
                var result = await controller.GetEnemiesByAttribute(pagination, attribute);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EnemiesEnvelope>>(result);
            }

            [Theory]
            [InlineData("boss", "bishop", 1)]
            [InlineData("floating", "banshee", 1)]
            [InlineData("ignore-liquids", "tarmonster", 1)]
            [InlineData("ignore-walls", "ghast", 1)]
            [InlineData("is-monkey-like", "gorgon", 1)]
            [InlineData("massive", "dead_ringer", 1)]
            [InlineData("miniboss", "banshee", 1)]
            public async Task ReturnsEnemiesFilteredByAttribute(string attribute, string name, int type)
            {
                // Arrange -> Act
                var result = await controller.GetEnemiesByAttribute(pagination, attribute);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                var first = contentEnemies.First();
                Assert.Equal(name, first.Name);
                Assert.Equal(type, first.Type);
            }
        }

        public class DisposeMethod : EnemiesControllerTests
        {
            public DisposeMethod(MockDatabaseFixture fixture) : base(fixture) { }

            [Fact]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [Fact]
            public void DisposeMoreThanOnce_DisposesDbOnlyOnce()
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

            [Fact]
            public async Task GetEnemiesMethod()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/enemies");

                // Assert
                await RespondsWithAsync(response, Resources.GetEnemies);
            }

            [Fact]
            public async Task GetEnemiesByAttributeMethod()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/enemies/boss");

                // Assert
                await RespondsWithAsync(response, Resources.GetEnemiesByAttribute);
            }
        }
    }
}
