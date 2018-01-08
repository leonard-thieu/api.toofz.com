using System.Linq;
using System.Net;
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
    public class EnemiesControllerTests
    {
        public EnemiesControllerTests(MockDatabaseFixture fixture)
        {
            var db = fixture.CreateNecroDancerContext();
            controller = new EnemiesController(db);
        }

        private readonly EnemiesController controller;

        public class Constructor
        {
            [DisplayFact(nameof(EnemiesController))]
            public void ReturnsEnemiesController()
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

            [DisplayFact(nameof(HttpStatusCode.OK))]
            public async Task ReturnsOK()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EnemiesEnvelope>>(result);
            }

            [DisplayFact]
            public async Task ReturnsEnemies()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.Equal(10, contentEnemies.Count());
            }

            [DisplayFact]
            public async Task ResultsAreOrderedByElemenyNameThenByType()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.Equal(new[]
                {
                    "armadillo", "armadillo", "armadillo",
                    "armoredskeleton", "armoredskeleton", "armoredskeleton", "armoredskeleton",
                    "banshee", "banshee",
                    "bat",
                }, contentEnemies.Select(e => e.Name));
                Assert.Equal(new[]
                {
                    1, 2, 3,
                    1, 2, 3, 4,
                    1, 2,
                    1,
                }, contentEnemies.Select(e => e.Type));
            }

            [DisplayFact]
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

            [DisplayFact]
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

            [DisplayTheory(nameof(HttpStatusCode.OK))]
            [InlineData("boss")]
            [InlineData("bounce-on-movement-fail")]
            [InlineData("floating")]
            [InlineData("ignore-liquids")]
            [InlineData("ignore-walls")]
            [InlineData("is-monkey-like")]
            [InlineData("massive")]
            [InlineData("miniboss")]
            public async Task ReturnsOK(string attribute)
            {
                // Arrange -> Act
                var result = await controller.GetEnemiesByAttribute(pagination, attribute);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EnemiesEnvelope>>(result);
            }

            [DisplayTheory]
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

        public class DisposeMethod
        {
            public DisposeMethod()
            {
                controller = new EnemiesController(mockDb.Object);
            }

            private readonly Mock<INecroDancerContext> mockDb = new Mock<INecroDancerContext>();
            private readonly EnemiesController controller;

            [DisplayFact]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [DisplayFact]
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

            [DisplayFact]
            public async Task GetEnemies()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/enemies");

                // Assert
                await RespondsWithAsync(response, Resources.GetEnemies);
            }

            [DisplayFact]
            public async Task GetEnemiesByAttribute()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/enemies/boss");

                // Assert
                await RespondsWithAsync(response, Resources.GetEnemiesByAttribute);
            }
        }
    }
}
