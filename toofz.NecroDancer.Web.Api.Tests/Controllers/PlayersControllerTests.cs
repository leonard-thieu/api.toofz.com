﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
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
                        Leaderboard = new Leaderboard { LeaderboardId = 739999 },
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
                var leaderboardHeaders = new LeaderboardHeaders();

                // Act
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);

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
                var leaderboardHeaders = LeaderboardsResources.ReadLeaderboardHeaders();
                controller = new PlayersController(db, storeClient, leaderboardHeaders);
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
                var leaderboardHeaders = new LeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);
                var steamId = 1;

                // Act
                var result = await controller.GetPlayerEntries(steamId);

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
                var leaderboardHeaders = new LeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);

                // Act
                var result = await controller.GetPlayerEntries(steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<PlayerEntriesDTO>));
                var contentResult = (OkNegotiatedContentResult<PlayerEntriesDTO>)result;
                Assert.IsNotNull(contentResult.Content);
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
                var leaderboardHeaders = new LeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);
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
                    Player = new Player { SteamId = steamId, },
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
                var leaderboardHeaders = new LeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<EntryDTO>));
                var contentResult = (OkNegotiatedContentResult<EntryDTO>)result;
                Assert.IsNotNull(contentResult.Content);
            }
        }

        [TestClass]
        public class PostPlayersMethod
        {
            [TestMethod]
            public async Task ReturnsBulkStoreDTO()
            {
                // Arrange
                var db = Mock.Of<ILeaderboardsContext>();
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                var leaderboardHeaders = LeaderboardsResources.ReadLeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);
                var players = new List<PlayerModel>();

                // Act
                var actionResult = await controller.PostPlayers(players);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<BulkStoreDTO>));
                var contentResult = (OkNegotiatedContentResult<BulkStoreDTO>)actionResult;
                Assert.IsNotNull(contentResult.Content);
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
                var leaderboardHeaders = new LeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);

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
                var leaderboardHeaders = new LeaderboardHeaders();
                var controller = new PlayersController(db, storeClient, leaderboardHeaders);

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
                var mockPlayers = new MockDbSet<Player>(Players);
                var players = mockPlayers.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(d => d.Players).Returns(players);
                var db = mockDb.Object;
                Kernel.Rebind<ILeaderboardsContext>().ToConstant(db);
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                Kernel.Rebind<ILeaderboardsStoreClient>().ToConstant(storeClient);

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
                var players = Players;
                var mockDbPlayers = new MockDbSet<Player>(players);
                var dbPlayers = mockDbPlayers.Object;
                var entries = from p in players
                              from e in p.Entries
                              select e;
                var mockDbEntries = new MockDbSet<Entry>(entries);
                var dbEntries = mockDbEntries.Object;
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(d => d.Players).Returns(dbPlayers);
                mockDb.Setup(d => d.Entries).Returns(dbEntries);
                mockDb.Setup(d => d.Replays).Returns(dbReplays);
                var db = mockDb.Object;
                Kernel.Rebind<ILeaderboardsContext>().ToConstant(db);
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                Kernel.Rebind<ILeaderboardsStoreClient>().ToConstant(storeClient);

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
                var players = Players;
                var entries = from p in players
                              from e in p.Entries
                              select e;
                var mockDbEntries = new MockDbSet<Entry>(entries);
                var dbEntries = mockDbEntries.Object;
                var mockDbReplays = new MockDbSet<Replay>();
                var dbReplays = mockDbReplays.Object;
                var mockDb = new Mock<ILeaderboardsContext>();
                mockDb.Setup(d => d.Entries).Returns(dbEntries);
                mockDb.Setup(d => d.Replays).Returns(dbReplays);
                var db = mockDb.Object;
                Kernel.Rebind<ILeaderboardsContext>().ToConstant(db);
                var storeClient = Mock.Of<ILeaderboardsStoreClient>();
                Kernel.Rebind<ILeaderboardsStoreClient>().ToConstant(storeClient);

                // Act
                var response = await Server.HttpClient.GetAsync("/players/2/entries/739999");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.That.NormalizedAreEqual(Resources.GetPlayerEntry, content);
            }
        }
    }
}
