using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class EnemiesControllerTests
    {
        private static IEnumerable<Enemy> Enemies
        {
            get => new[]
            {
                new Enemy("monkey", 4)
                {
                    Stats = new Stats
                    {
                        Health = 2,
                        DamagePerHit = 0,
                        BeatsPerMove = 1,
                        CoinsToDrop = 2,
                    },
                    OptionalStats = new OptionalStats { IsMonkeyLike = true },
                    DisplayName = "Magic Monkey",
                },
                new Enemy("necrodancer", 1)
                {
                    Stats = new Stats
                    {
                        Health = 6,
                        DamagePerHit = 3,
                        BeatsPerMove = 2,
                        CoinsToDrop = 0,
                    },
                    OptionalStats = new OptionalStats { Boss = true },
                    DisplayName = "The Necrodancer",
                },
                new Enemy("bat", 1)
                {
                    Stats = new Stats
                    {
                        Health = 1,
                        DamagePerHit = 1,
                        BeatsPerMove = 2,
                        CoinsToDrop = 2,
                    },
                    OptionalStats = new OptionalStats { Floating = true },
                    DisplayName = "Blue Bat",
                },
                new Enemy("tarmonster", 1)
                {
                    Stats = new Stats
                    {
                        Health = 1,
                        DamagePerHit = 3,
                        BeatsPerMove = 1,
                        CoinsToDrop = 3,
                    },
                    OptionalStats = new OptionalStats { IgnoreLiquids = true },
                    DisplayName = "Tarmonster",
                },
                new Enemy("spider", 1)
                {
                    Stats = new Stats
                    {
                        Health = 1,
                        DamagePerHit = 2,
                        BeatsPerMove = 2,
                        CoinsToDrop = 3,
                    },
                    OptionalStats = new OptionalStats { IgnoreWalls = true },
                    DisplayName = "Spider",
                },
                new Enemy("monkey", 3)
                {
                    Stats = new Stats
                    {
                        Health = 1,
                        DamagePerHit = 0,
                        BeatsPerMove = 1,
                        CoinsToDrop = 1,
                    },
                    OptionalStats = new OptionalStats { IsMonkeyLike = true },
                    DisplayName = "Green Monkey",
                },
                new Enemy("dragon", 2)
                {
                    Stats = new Stats
                    {
                        Health = 6,
                        DamagePerHit = 6,
                        BeatsPerMove = 2,
                        CoinsToDrop = 20,
                    },
                    OptionalStats = new OptionalStats { Massive = true },
                    DisplayName = "Red Dragon",
                },
                new Enemy("ogre", 1)
                {
                    Stats = new Stats
                    {
                        Health = 5,
                        DamagePerHit = 5,
                        BeatsPerMove = 4,
                        CoinsToDrop = 15,
                    },
                    OptionalStats = new OptionalStats { Miniboss = true },
                    DisplayName = "Ogre",
                },
            };
        }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var mockDb = new Mock<INecroDancerContext>();
                var db = mockDb.Object;

                // Act
                var controller = new EnemiesController(db);

                // Assert
                Assert.IsAssignableFrom<EnemiesController>(controller);
            }
        }

        public class GetEnemiesMethod
        {
            public GetEnemiesMethod()
            {
                var dbEnemies = new FakeDbSet<Enemy>(Enemies);
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.Setup(x => x.Enemies).Returns(dbEnemies);
                var db = mockDb.Object;
                controller = new EnemiesController(db);
                pagination = new EnemiesPagination();
            }

            private EnemiesController controller;
            private EnemiesPagination pagination;

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
                Assert.Equal(8, contentEnemies.Count());
            }

            [Fact]
            public async Task ResultsAreOrderedByElemenyNameThenByType()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                var first = contentEnemies.First();
                Assert.Equal("bat", first.Name);
                Assert.Equal(1, first.Type);
                var firstMonkey = contentEnemies.First(e => e.Name == "monkey");
                Assert.Equal("monkey", firstMonkey.Name);
                Assert.Equal(3, firstMonkey.Type);
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
                Assert.Equal("monkey", first.Name);
                Assert.Equal(3, first.Type);
            }
        }

        public class GetEnemiesByAttributeMethod
        {
            public GetEnemiesByAttributeMethod()
            {
                var mockDb = new Mock<INecroDancerContext>();
                var db = mockDb.Object;
                var dbEnemies = new FakeDbSet<Enemy>(Enemies);
                mockDb.Setup(x => x.Enemies).Returns(dbEnemies);
                controller = new EnemiesController(db);
                pagination = new EnemiesPagination();
            }

            private EnemiesController controller;
            private EnemiesPagination pagination;

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
            [InlineData("boss", "necrodancer", 1)]
            [InlineData("floating", "bat", 1)]
            [InlineData("ignore-liquids", "tarmonster", 1)]
            [InlineData("ignore-walls", "spider", 1)]
            [InlineData("is-monkey-like", "monkey", 3)]
            [InlineData("massive", "dragon", 2)]
            [InlineData("miniboss", "ogre", 1)]
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
            [Fact]
            public void DisposesDb()
            {
                // Arrange
                var mockDb = new Mock<INecroDancerContext>();
                var db = mockDb.Object;
                var controller = new EnemiesController(db);

                // Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [Fact]
            public void DisposeMoreThanOnce_DisposesDbOnlyOnce()
            {
                // Arrange
                var mockDb = new Mock<INecroDancerContext>();
                var db = mockDb.Object;
                var controller = new EnemiesController(db);

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
