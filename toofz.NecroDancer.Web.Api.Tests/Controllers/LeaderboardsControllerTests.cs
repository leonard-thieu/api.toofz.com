using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class LeaderboardsControllerTests
    {
        static IEnumerable<Leaderboard> Leaderboards
        {
            get
            {
                return new[]
                {
                    new Leaderboard
                    {
                        Product = new Product(1, "amplified", "Amplified"),
                        Mode = new Mode(0, "standard", "Standard"),
                        Run = new Run(1, "speed", "Speed"),
                        Character = new Character(0, "cadence", "Cadence"),
                    },
                };
            }
        }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var db = Mock.Of<ILeaderboardsContext>();

                // Act
                var controller = new LeaderboardsController(db);

                // Assert
                Assert.IsInstanceOfType(controller, typeof(LeaderboardsController));
            }
        }

        [TestClass]
        public class GetLeaderboardsMethod
        {
            [TestMethod]
            public async Task ReturnsLeaderboards()
            {
                // Arrange
                var mockLeaderboards = new MockDbSet<Leaderboard>();
                var leaderboards = mockLeaderboards.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(d => d.Leaderboards).Returns(leaderboards);
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);
                var products = new Products(LeaderboardCategories.Products.Select(p => p.Name).ToList());
                var modes = new Modes(LeaderboardCategories.Modes.Select(m => m.Name).ToList());
                var runs = new Runs(LeaderboardCategories.Runs.Select(r => r.Name).ToList());
                var characters = new Characters(LeaderboardCategories.Characters.Select(c => c.Name).ToList());

                // Act
                var result = await controller.GetLeaderboards(products, modes, runs, characters);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LeaderboardsEnvelope>));
            }
        }

        [TestClass]
        public class GetLeaderboardEntriesMethod
        {
            [TestMethod]
            public async Task ReturnsLeaderboardEntries()
            {
                // Arrange
                var mockDbLeaderboards = new MockDbSet<Leaderboard>(
                    new Leaderboard
                    {
                        LeaderboardId = 741312,
                        Product = new Product(1, "myName", "myDisplayName"),
                        Mode = new Mode(1, "myName", "myDisplayName"),
                        Run = new Run(1, "myName", "myDisplayName"),
                        Character = new Character(1, "myName", "myDisplayName"),
                    }
                );
                var dbLeaderboards = mockDbLeaderboards.Object;
                var mockDbEntries = new MockDbSet<Entry>();
                var dbEntries = mockDbEntries.Object;
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb.Setup(x => x.Leaderboards).Returns(dbLeaderboards);
                mockDb.Setup(x => x.Entries).Returns(dbEntries);
                mockDb.Setup(x => x.Replays).Returns(dbReplays);
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();

                // Act
                var result = await controller.GetLeaderboardEntries(pagination, 741312);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<LeaderboardEntriesDTO>));
            }

            [TestMethod]
            public async Task LbidNotInDb_ReturnsNotFound()
            {
                // Arrange
                var mockDbLeaderboards = new MockDbSet<Leaderboard>(new Leaderboard { LeaderboardId = 22 });
                var dbLeaderboards = mockDbLeaderboards.Object;
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb.Setup(x => x.Leaderboards).Returns(dbLeaderboards);
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();

                // Act
                var actionResult = await controller.GetLeaderboardEntries(pagination, 0);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
            }
        }

        [TestClass]
        public class GetDailyLeaderboardsMethod
        {
            [TestMethod]
            public async Task ReturnsDailyLeaderboards()
            {
                // Arrange
                var db = Util.CreateLeaderboardsContext();
                var mockDb = Mock.Get(db);
                var mockDbDailyLeaderboards = new MockDbSet<DailyLeaderboard>();
                var dbDailyLeaderboards = mockDbDailyLeaderboards.Object;
                mockDb.Setup(x => x.DailyLeaderboards).Returns(dbDailyLeaderboards);
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();
                var productsParams = new Products(LeaderboardCategories.Products.Select(p => p.Name).ToList());

                // Act
                var result = await controller.GetDailyLeaderboards(pagination, productsParams);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyLeaderboardsEnvelope>));
            }
        }

        [TestClass]
        public class GetDailyLeaderboardEntriesMethod
        {
            [TestMethod]
            public async Task ReturnsDailyLeaderboardEntries()
            {
                // Arrange
                var mockDbDailyLeaderboards = new MockDbSet<DailyLeaderboard>(
                    new DailyLeaderboard
                    {
                        LeaderboardId = 1,
                        Product = new Product(1, "myName", "myDisplayName"),
                    }
                );
                var dbDailyLeaderboards = mockDbDailyLeaderboards.Object;
                var mockDbDailyEntries = new MockDbSet<DailyEntry>();
                var dbDailyEntries = mockDbDailyEntries.Object;
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb.Setup(d => d.DailyLeaderboards).Returns(dbDailyLeaderboards);
                mockDb.Setup(d => d.DailyEntries).Returns(dbDailyEntries);
                mockDb.Setup(d => d.Replays).Returns(dbReplays);
                var db = mockDb.Object;
                var controller = new LeaderboardsController(db);
                var pagination = new LeaderboardsPagination();
                var lbid = 1;

                // Act
                var result = await controller.GetDailyLeaderboardEntries(pagination, lbid);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyLeaderboardEntriesDTO>));
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
                var controller = new LeaderboardsController(db);

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
                var controller = new LeaderboardsController(db);

                // Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        [TestClass]
        public class IntegrationTests : IntegrationTestsBase
        {
            [TestMethod]
            public async Task GetLeaderboardsMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var leaderboards = Leaderboards;
                var mockDbLeaderboards = new MockDbSet<Leaderboard>(leaderboards);
                var dbLeaderboards = mockDbLeaderboards.Object;
                mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);
                Kernel.Rebind<ILeaderboardsContext>().ToConstant(db);

                // Act
                var response = await Server.HttpClient.GetAsync("/leaderboards");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetLeaderboards, content);
            }
        }
    }
}
