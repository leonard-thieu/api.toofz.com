using System;
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
    class PlayersControllerTests
    {
        static IEnumerable<Player> Players
        {
            get
            {
                var p1 = new Player
                {
                    SteamId = 1,
                    Name = "Leonard",
                };
                p1.Entries.AddRange(new[]
                {
                    new Entry
                    {
                        Leaderboard = new Leaderboard
                        {
                            LeaderboardId = 739999,
                            Product = new Product(1, "classic", "Classic"),
                            Mode = new Mode(1, "standard", "Standard"),
                            Run = new Run(1, "score", "Score"),
                            Character = new Character(1, "cadence", "Cadence"),
                        },
                        LeaderboardId = 739999,
                        Player = p1,
                        SteamId = 1,
                        ReplayId = 421438228643743438,
                    },
                });
                p1.DailyEntries.AddRange(new[]
                {
                    new DailyEntry
                    {
                        Leaderboard = new DailyLeaderboard
                        {
                            LeaderboardId = 739999,
                            Product = new Product(1, "classic", "Classic"),
                        },
                        LeaderboardId = 739999,
                        Player = p1,
                        SteamId = 1,
                        ReplayId = 421438228643743438,
                    },
                });
                var p2 = new Player
                {
                    SteamId = 2,
                    Name = "Julianna",
                };
                p2.Entries.AddRange(new[]
                {
                    new Entry
                    {
                        Leaderboard = new Leaderboard { LeaderboardId = 739795 },
                        LeaderboardId = 739795,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 26236580330596509,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard { LeaderboardId = 739796 },
                        LeaderboardId = 739796,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 26236580330596596,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard { LeaderboardId = 739999 },
                        LeaderboardId = 739999,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 422567813941329155,
                    },
                });
                p2.DailyEntries.AddRange(new[]
                {
                    new DailyEntry
                    {
                        Leaderboard = new DailyLeaderboard
                        {
                            LeaderboardId = 739999,
                            Product = new Product(1, "classic", "Classic"),
                        },
                        LeaderboardId = 739999,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 421438228643743438,
                    },
                });
                var p3 = new Player
                {
                    SteamId = 3,
                    Name = "Steve",
                };
                p3.Entries.AddRange(new[]
                {
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 3,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 3,
                    },
                });
                var p4 = new Player
                {
                    SteamId = 4,
                    Name = "Julianna",
                };
                p4.Entries.AddRange(new[]
                {
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 4,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 4,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 4,
                    },
                });
                var p5 = new Player
                {
                    SteamId = 5,
                    Name = "Stacey",
                };
                p5.Entries.AddRange(new[]
                {
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 5,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 5,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 5,
                    },
                });

                return new[] { p1, p2, p3, p4, p5 };
            }
        }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var db = new LeaderboardsContext();
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();

                // Act
                var controller = new PlayersController(db, storeClient);

                // Assert
                Assert.IsInstanceOfType(controller, typeof(PlayersController));
            }
        }

        [TestClass]
        public class GetPlayersMethod
        {
            public GetPlayersMethod()
            {
                var mockPlayers = new MockDbSet<Player>(Players);
                var players = mockPlayers.Object;
                var mockDb = new Mock<LeaderboardsContext>();
                mockDb.Setup(d => d.Players).Returns(players);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                controller = new PlayersController(db, storeClient);
            }

            PlayersController controller;

            [TestMethod]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                var pagination = new PlayersPagination { Limit = 2 };
                var sort = new PlayersSortParams();

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayersEnvelope>));
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "1", "2" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                CollectionAssert.AreEqual(expectedIds, actualIds);
            }

            [TestMethod]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                var pagination = new PlayersPagination { Offset = 2 };
                var sort = new PlayersSortParams();

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayersEnvelope>));
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "3", "4", "5" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                CollectionAssert.AreEqual(expectedIds, actualIds);
            }

            [TestMethod]
            public async Task SortIsSpecified_ReturnsSortedResults()
            {
                // Arrange
                var pagination = new PlayersPagination();
                var sort = new PlayersSortParams();
                sort.Add("-id");

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayersEnvelope>));
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "5", "4", "3", "2", "1" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                CollectionAssert.AreEqual(expectedIds, actualIds);
            }

            [TestMethod]
            public async Task QIsSpecified_ReturnsFilteredPlayers()
            {
                // Arrange
                var pagination = new PlayersPagination();
                var sort = new PlayersSortParams();
                var q = "Sta";

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort, q);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayersEnvelope>));
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "5" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                CollectionAssert.AreEqual(expectedIds, actualIds);
            }

            [TestMethod]
            public async Task QIsNotSpecified_ReturnsPlayers()
            {
                // Arrange
                var pagination = new PlayersPagination();
                var sort = new PlayersSortParams();

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayersEnvelope>));
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                Assert.AreEqual(5, playerDTOs.Count());
            }
        }

        [TestClass]
        public class GetPlayerEntriesMethod
        {
            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                var mockPlayers = new MockDbSet<Player>();
                var players = mockPlayers.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Players).Returns(players);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);
                var steamId = 1;
                var products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

                // Act
                var result = await controller.GetPlayerEntries(steamId, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                var steamId = 76561197960481221;
                var mockPlayers = new MockDbSet<Player>(new Player { SteamId = steamId });
                var players = mockPlayers.Object;
                var mockEntries = new MockDbSet<Entry>();
                var entries = mockEntries.Object;
                var mockReplays = new MockDbSet<Replay>();
                var replays = mockReplays.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Players).Returns(players);
                mockDb.Setup(x => x.Entries).Returns(entries);
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);
                var products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

                // Act
                var result = await controller.GetPlayerEntries(steamId, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PlayerEntriesDTO>));
            }
        }

        [TestClass]
        public class GetPlayerEntryMethod
        {
            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                var mockEntries = new MockDbSet<Entry>();
                var entries = mockEntries.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Entries).Returns(entries);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);
                var lbid = 234265;
                var steamId = 1;

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntry()
            {
                // Arrange
                var lbid = 234265;
                var steamId = 1;
                var entry = new Entry
                {
                    LeaderboardId = lbid,
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                };
                var mockEntries = new MockDbSet<Entry>(entry);
                var entries = mockEntries.Object;
                var mockReplays = new MockDbSet<Replay>();
                var replays = mockReplays.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Entries).Returns(entries);
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EntryDTO>));
            }

            [TestMethod]
            public async Task ReplayFound_AddsReplayInformation()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var lbid = 234265;
                var steamId = 1;
                var entry = new Entry
                {
                    LeaderboardId = lbid,
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                    ReplayId = 234,
                };
                var mockEntries = new MockDbSet<Entry>(entry);
                var entries = mockEntries.Object;
                mockDb.Setup(x => x.Entries).Returns(entries);
                var replay = new Replay
                {
                    ReplayId = 234,
                    Version = 74,
                    KilledBy = "BOMB",
                };
                var mockReplays = new MockDbSet<Replay>(replay);
                var replays = mockReplays.Object;
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EntryDTO>));
                var contentResult = (OkNegotiatedContentResult<EntryDTO>)result;
                var content = contentResult.Content;
                Assert.AreEqual(74, content.Version);
                Assert.AreEqual("BOMB", content.KilledBy);
            }
        }

        [TestClass]
        public class GetPlayerDailyEntriesMethod
        {
            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                var mockPlayers = new MockDbSet<Player>();
                var players = mockPlayers.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Players).Returns(players);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);
                var steamId = 1;
                var products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                var steamId = 76561197960481221;
                var mockPlayers = new MockDbSet<Player>(new Player { SteamId = steamId });
                var players = mockPlayers.Object;
                var mockEntries = new MockDbSet<DailyEntry>();
                var entries = mockEntries.Object;
                var mockReplays = new MockDbSet<Replay>();
                var replays = mockReplays.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.Players).Returns(players);
                mockDb.Setup(x => x.DailyEntries).Returns(entries);
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);
                var products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PlayerDailyEntriesDTO>));
            }
        }

        [TestClass]
        public class GetPlayerDailyEntryMethod
        {
            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                var mockEntries = new MockDbSet<DailyEntry>();
                var entries = mockEntries.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(x => x.DailyEntries).Returns(entries);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);
                var lbid = 234265;
                var steamId = 1;

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntry()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var lbid = 234265;
                var steamId = 1;
                var entry = new DailyEntry
                {
                    LeaderboardId = lbid,
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                };
                var mockEntries = new MockDbSet<DailyEntry>(entry);
                var entries = mockEntries.Object;
                mockDb.Setup(x => x.DailyEntries).Returns(entries);
                var mockReplays = new MockDbSet<Replay>();
                var replays = mockReplays.Object;
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyEntryDTO>));
            }

            [TestMethod]
            public async Task ReplayFound_AddsReplayInformation()
            {
                // Arrange
                var mockDb = new Mock<ILeaderboardsContext>();
                var lbid = 234265;
                var steamId = 1;
                var entry = new DailyEntry
                {
                    LeaderboardId = lbid,
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                    ReplayId = 234,
                };
                var mockEntries = new MockDbSet<DailyEntry>(entry);
                var entries = mockEntries.Object;
                mockDb.Setup(x => x.DailyEntries).Returns(entries);
                var replay = new Replay
                {
                    ReplayId = 234,
                    Version = 74,
                    KilledBy = "BOMB",
                };
                var mockReplays = new MockDbSet<Replay>(replay);
                var replays = mockReplays.Object;
                mockDb.Setup(x => x.Replays).Returns(replays);
                var db = mockDb.Object;
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var controller = new PlayersController(db, storeClient);

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyEntryDTO>));
                var contentResult = (OkNegotiatedContentResult<DailyEntryDTO>)result;
                var content = contentResult.Content;
                Assert.AreEqual(74, content.Version);
                Assert.AreEqual("BOMB", content.KilledBy);
            }
        }

        [TestClass]
        public class PostPlayersMethod
        {
            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                var players = new List<PlayerModel>
                {
                    new PlayerModel
                    {
                        SteamId = 1,
                        Exists = true,
                        Name = "Mendayen",
                        LastUpdate = new DateTime(2017, 9, 30, 21, 45, 53, DateTimeKind.Utc),
                        Avatar = "http://example.org/"
                    },
                };
                var db = Mock.Of<ILeaderboardsContext>();
                var mockStoreClient = new Mock<ILeaderboardsStoreClient>();
                mockStoreClient.Setup(s => s.SaveChangesAsync(It.IsAny<IEnumerable<Player>>(), true, default)).Returns(Task.FromResult(players.Count));
                var storeClient = mockStoreClient.Object;
                var controller = new PlayersController(db, storeClient);

                // Act
                var actionResult = await controller.PostPlayers(players);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<BulkStoreDTO>));
                var contentResult = (OkNegotiatedContentResult<BulkStoreDTO>)actionResult;
                var content = contentResult.Content;
                Assert.AreEqual(1, content.RowsAffected);
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
                var controller = new PlayersController(db, storeClient);

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
                var controller = new PlayersController(db, storeClient);

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
            public async Task GetPlayersMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var mockPlayers = new MockDbSet<Player>(Players);
                var players = mockPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(players);

                // Act
                var response = await Server.HttpClient.GetAsync("/players?q=St");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetPlayers, content);
            }

            [TestMethod]
            public async Task GetPlayerEntriesMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                var entries = from p in players
                              from e in p.Entries
                              select e;
                var mockDbEntries = new MockDbSet<Entry>(entries);
                var dbEntries = mockDbEntries.Object;
                mockDb.Setup(d => d.Entries).Returns(dbEntries);
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                mockDb.Setup(d => d.Replays).Returns(dbReplays);

                // Act
                var response = await Server.HttpClient.GetAsync("/players/1/entries");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetPlayerEntries, content);
            }

            [TestMethod]
            public async Task GetPlayerEntryMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var entries = from p in players
                              from e in p.Entries
                              select e;
                var mockDbEntries = new MockDbSet<Entry>(entries);
                var dbEntries = mockDbEntries.Object;
                mockDb.Setup(d => d.Entries).Returns(dbEntries);
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                mockDb.Setup(d => d.Replays).Returns(dbReplays);

                // Act
                var response = await Server.HttpClient.GetAsync("/players/2/entries/739999");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetPlayerEntry, content);
            }

            [TestMethod]
            public async Task GetPlayerDailyEntriesMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                var entries = from p in players
                              from e in p.DailyEntries
                              select e;
                var mockDbEntries = new MockDbSet<DailyEntry>(entries);
                var dbEntries = mockDbEntries.Object;
                mockDb.Setup(d => d.DailyEntries).Returns(dbEntries);
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                mockDb.Setup(d => d.Replays).Returns(dbReplays);

                // Act
                var response = await Server.HttpClient.GetAsync("/players/1/entries/dailies");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetPlayerDailyEntries, content);
            }

            [TestMethod]
            public async Task GetPlayerDailyEntryMethod()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var entries = from p in players
                              from e in p.DailyEntries
                              select e;
                var mockDbEntries = new MockDbSet<DailyEntry>(entries);
                var dbEntries = mockDbEntries.Object;
                mockDb.Setup(d => d.DailyEntries).Returns(dbEntries);
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                mockDb.Setup(d => d.Replays).Returns(dbReplays);

                // Act
                var response = await Server.HttpClient.GetAsync("/players/2/entries/dailies/739999");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetPlayerEntry, content);
            }
        }
    }
}
