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
                        Leaderboard = new Leaderboard
                        {
                            LeaderboardId = 739795,
                            Product = new Product(1, "classic", "Classic"),
                            Mode = new Mode(1, "standard", "Standard"),
                            Run = new Run(1, "score", "Score"),
                            Character = new Character(1, "cadence", "Cadence"),
                        },
                        LeaderboardId = 739795,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 26236580330596509,
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard
                        {
                            LeaderboardId = 739796,
                            Product = new Product(1, "classic", "Classic"),
                            Mode = new Mode(1, "standard", "Standard"),
                            Run = new Run(1, "score", "Score"),
                            Character = new Character(1, "cadence", "Cadence"),
                        },
                        LeaderboardId = 739796,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 26236580330596596,
                    },
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
                            LeaderboardId = 739795,
                            Product = new Product(1, "classic", "Classic"),
                        },
                        LeaderboardId = 739795,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 26236580330596509,
                    },
                    new DailyEntry
                    {
                        Leaderboard = new DailyLeaderboard
                        {
                            LeaderboardId = 739796,
                            Product = new Product(1, "classic", "Classic"),
                        },
                        LeaderboardId = 739796,
                        Player = p2,
                        SteamId = 2,
                        ReplayId = 26236580330596596,
                    },
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

        public PlayersControllerTests()
        {
            storeClient = mockStoreClient.Object;
            controller = new PlayersController(mockDb.Db, storeClient);
        }

        protected MockLeaderboardsContext mockDb = new MockLeaderboardsContext();
        protected Mock<ILeaderboardsStoreClient> mockStoreClient = new Mock<ILeaderboardsStoreClient>();
        protected ILeaderboardsStoreClient storeClient;
        protected PlayersController controller;

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
        public class GetPlayersMethod : PlayersControllerTests
        {
            public GetPlayersMethod()
            {
                mockDb.Players.AddRange(Players);
            }

            protected PlayersPagination pagination = new PlayersPagination();
            protected PlayersSortParams sort = new PlayersSortParams();

            [TestMethod]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.Limit = 2;

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
                pagination.Offset = 2;

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
                // Arrange -> Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<PlayersEnvelope>));
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                Assert.AreEqual(5, playerDTOs.Count());
            }
        }

        [TestClass]
        public class GetPlayerMethod : PlayersControllerTests
        {
            protected long steamId;

            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayer(steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                steamId = 76561197960481221;
                mockDb.Players.Add(new Player { SteamId = steamId });

                // Act
                var result = await controller.GetPlayer(steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PlayerEnvelope>));
            }
        }

        [TestClass]
        public class GetPlayerEntriesMethod : PlayersControllerTests
        {
            protected long steamId;
            protected LeaderboardIdParams lbids = new LeaderboardIdParams();
            protected Products products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayerEntries(steamId, lbids, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                steamId = 76561197960481221;
                mockDb.Players.Add(new Player { SteamId = steamId });

                // Act
                var result = await controller.GetPlayerEntries(steamId, lbids, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PlayerEntriesDTO>));
            }
        }

        [TestClass]
        public class GetPlayerEntryMethod : PlayersControllerTests
        {
            protected int lbid;
            protected int steamId;

            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntry()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;
                var entry = new Entry
                {
                    LeaderboardId = lbid,
                    Leaderboard = new Leaderboard
                    {
                        LeaderboardId = lbid,
                        Product = new Product(0, "", ""),
                        Mode = new Mode(0, "", ""),
                        Run = new Run(0, "", ""),
                        Character = new Character(0, "", ""),
                    },
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                };
                mockDb.Entries.Add(entry);

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EntryDTO>));
            }

            [TestMethod]
            public async Task ReplayFound_AddsReplayInformation()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;
                var entry = new Entry
                {
                    LeaderboardId = lbid,
                    Leaderboard = new Leaderboard
                    {
                        LeaderboardId = lbid,
                        Product = new Product(0, "", ""),
                        Mode = new Mode(0, "", ""),
                        Run = new Run(0, "", ""),
                        Character = new Character(0, "", ""),
                    },
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                    ReplayId = 234,
                };
                mockDb.Entries.Add(entry);
                var replay = new Replay
                {
                    ReplayId = 234,
                    Version = 74,
                    KilledBy = "BOMB",
                };
                mockDb.Replays.Add(replay);

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
        public class GetPlayerDailyEntriesMethod : PlayersControllerTests
        {
            protected long steamId;
            protected LeaderboardIdParams leaderboardIds = new LeaderboardIdParams();
            protected Products products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, leaderboardIds, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                var steamId = 76561197960481221;
                mockDb.Players.Add(new Player { SteamId = steamId });

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, leaderboardIds, products);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PlayerDailyEntriesDTO>));
            }
        }

        [TestClass]
        public class GetPlayerDailyEntryMethod : PlayersControllerTests
        {
            protected int lbid;
            protected long steamId;

            [TestMethod]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [TestMethod]
            public async Task ReturnsPlayerEntry()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;
                var entry = new DailyEntry
                {
                    LeaderboardId = lbid,
                    Leaderboard = new DailyLeaderboard
                    {
                        LeaderboardId = lbid,
                        Product = new Product(0, "", ""),
                    },
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                };
                mockDb.DailyEntries.Add(entry);

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DailyEntryDTO>));
            }

            [TestMethod]
            public async Task ReplayFound_AddsReplayInformation()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;
                var entry = new DailyEntry
                {
                    LeaderboardId = lbid,
                    Leaderboard = new DailyLeaderboard
                    {
                        LeaderboardId = lbid,
                        Product = new Product(0, "", ""),
                    },
                    SteamId = steamId,
                    Player = new Player { SteamId = steamId },
                    ReplayId = 234,
                };
                mockDb.DailyEntries.Add(entry);
                var replay = new Replay
                {
                    ReplayId = 234,
                    Version = 74,
                    KilledBy = "BOMB",
                };
                mockDb.Replays.Add(replay);

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
        public class PostPlayersMethod : PlayersControllerTests
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
                mockStoreClient.Setup(s => s.SaveChangesAsync(It.IsAny<IEnumerable<Player>>(), true, default)).ReturnsAsync(players.Count);

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
        public class DisposeMethod : PlayersControllerTests
        {
            [TestMethod]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.MockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [TestMethod]
            public void DisposesMoreThanOnce_OnlyDisposesDbOnce()
            {
                // Arrange -> Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.MockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        [TestClass]
        public class IntegrationTests : IntegrationTestsBase
        {
            [TestMethod]
            public async Task GetPlayers()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var mockPlayers = new MockDbSet<Player>(Players);
                var players = mockPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(players);

                // Act
                var response = await Server.HttpClient.GetAsync("/players?q=St");

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayers);
            }

            [TestMethod]
            public async Task GetPlayer()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);

                // Act
                var response = await Server.HttpClient.GetAsync("/players/1");

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayer);
            }

            [TestMethod]
            public async Task GetPlayerEntries()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                var mockDbLeaderboards = new MockDbSet<Leaderboard>();
                var dbLeaderboards = mockDbLeaderboards.Object;
                mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);
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

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayerEntries);
            }

            [TestMethod]
            public async Task GetPlayerEntriesFilteredByLbids()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                var leaderboards = from p in players
                                   from e in p.Entries
                                   select e.Leaderboard;
                var mockDbLeaderboards = new MockDbSet<Leaderboard>(leaderboards);
                var dbLeaderboards = mockDbLeaderboards.Object;
                mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);
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
                var response = await Server.HttpClient.GetAsync("/players/2/entries?lbids=739796,739999");

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayerEntriesFilteredByLbids);
            }

            [TestMethod]
            public async Task GetPlayerEntry()
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

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayerEntry);
            }

            [TestMethod]
            public async Task GetPlayerDailyEntries()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                var mockDbLeaderboards = new MockDbSet<DailyLeaderboard>();
                var dbLeaderboards = mockDbLeaderboards.Object;
                mockDb.Setup(d => d.DailyLeaderboards).Returns(dbLeaderboards);
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

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayerDailyEntries);
            }

            [TestMethod]
            public async Task GetPlayerDailyEntriesFilteredByLbids()
            {
                // Arrange
                var db = Kernel.Get<ILeaderboardsContext>();
                var mockDb = Mock.Get(db);
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                var leaderboards = from p in players
                                   from e in p.DailyEntries
                                   select e.Leaderboard;
                var mockDbLeaderboards = new MockDbSet<DailyLeaderboard>(leaderboards);
                var dbLeaderboards = mockDbLeaderboards.Object;
                mockDb.Setup(d => d.DailyLeaderboards).Returns(dbLeaderboards);
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
                var response = await Server.HttpClient.GetAsync("/players/2/entries/dailies?lbids=739796,739999");

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayerDailyEntriesFilteredByLbids);
            }

            [TestMethod]
            public async Task GetPlayerDailyEntry()
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

                // Assert
                await Assert.That.RespondsWithAsync(response, Resources.GetPlayerDailyEntry);
            }
        }
    }
}
