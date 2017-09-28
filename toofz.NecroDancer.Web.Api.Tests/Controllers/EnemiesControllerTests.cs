using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class EnemiesControllerTests
    {
        static IEnumerable<Enemy> GetEnemies()
        {
            return new List<Enemy>
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
                },
            };
        }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var mockDb = new Mock<NecroDancerContext>();
                var db = mockDb.Object;

                // Act
                var controller = new EnemiesController(db);

                // Assert
                Assert.IsInstanceOfType(controller, typeof(EnemiesController));
            }
        }

        [TestClass]
        public class GetEnemiesMethod_EnemiesPagination_CancellationToken
        {
            public GetEnemiesMethod_EnemiesPagination_CancellationToken()
            {
                var enemies = GetEnemies();
                var mockDbEnemies = new MockDbSet<Enemy>(enemies);
                var dbEnemies = mockDbEnemies.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Enemies).Returns(dbEnemies);
                var db = mockDb.Object;
                controller = new EnemiesController(db);
                pagination = new EnemiesPagination();
            }

            EnemiesController controller;
            EnemiesPagination pagination;

            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EnemiesEnvelope>));
            }

            [TestMethod]
            public async Task ReturnsEnemies()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.AreEqual(8, contentEnemies.Count());
            }

            [TestMethod]
            public async Task ResultsAreOrderedByElemenyNameThenByType()
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                var first = contentEnemies.First();
                Assert.AreEqual("bat", first.Name);
                Assert.AreEqual(1, first.Type);
                var firstMonkey = contentEnemies.First(e => e.Name == "monkey");
                Assert.AreEqual("monkey", firstMonkey.Name);
                Assert.AreEqual(3, firstMonkey.Type);
            }

            [TestMethod]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.limit = 2;

                // Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                Assert.AreEqual(2, contentEnemies.Count());
            }

            [TestMethod]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                pagination.offset = 2;

                // Act
                var result = await controller.GetEnemies(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                var first = contentEnemies.First();
                Assert.AreEqual("monkey", first.Name);
                Assert.AreEqual(3, first.Type);
            }
        }

        [TestClass]
        public class GetEnemiesMethod_EnemiesPagination_String_CancellationToken
        {
            public GetEnemiesMethod_EnemiesPagination_String_CancellationToken()
            {
                var enemies = GetEnemies();
                var mockDbEnemies = new MockDbSet<Enemy>(enemies);
                var dbEnemies = mockDbEnemies.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Enemies).Returns(dbEnemies);
                var db = mockDb.Object;
                controller = new EnemiesController(db);
                pagination = new EnemiesPagination();
            }

            EnemiesController controller;
            EnemiesPagination pagination;

            [DataTestMethod]
            [DataRow("boss")]
            [DataRow("bounce-on-movement-fail")]
            [DataRow("floating")]
            [DataRow("ignore-liquids")]
            [DataRow("ignore-walls")]
            [DataRow("is-monkey-like")]
            [DataRow("massive")]
            [DataRow("miniboss")]
            public async Task ReturnsOk(string attribute)
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination, attribute);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EnemiesEnvelope>));
            }

            [DataTestMethod]
            [DataRow("boss", "necrodancer", 1)]
            [DataRow("floating", "bat", 1)]
            [DataRow("ignore-liquids", "tarmonster", 1)]
            [DataRow("ignore-walls", "spider", 1)]
            [DataRow("is-monkey-like", "monkey", 3)]
            [DataRow("massive", "dragon", 2)]
            [DataRow("miniboss", "ogre", 1)]
            public async Task ReturnsEnemiesFilteredByAttribute(string attribute, string name, int type)
            {
                // Arrange -> Act
                var result = await controller.GetEnemies(pagination, attribute);

                // Assert
                var contentResult = (OkNegotiatedContentResult<EnemiesEnvelope>)result;
                var contentEnemies = contentResult.Content.Enemies;
                var first = contentEnemies.First();
                Assert.AreEqual(name, first.Name);
                Assert.AreEqual(type, first.Type);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
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

            [TestMethod]
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
    }
}
