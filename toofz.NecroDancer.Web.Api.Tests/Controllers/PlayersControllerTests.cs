using System.Collections.Generic;
using System.Linq;
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
    public class PlayersControllerTests
    {
        private static IEnumerable<Player> Players
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
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
                        Replay = new Replay(),
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 3,
                        Replay = new Replay(),
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
                        Replay = new Replay(),
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 4,
                        Replay = new Replay(),
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 4,
                        Replay = new Replay(),
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
                        Replay = new Replay(),
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 5,
                        Replay = new Replay(),
                    },
                    new Entry
                    {
                        Leaderboard = new Leaderboard(),
                        SteamId = 5,
                        Replay = new Replay(),
                    },
                });

                return new[] { p1, p2, p3, p4, p5 };
            }
        }

        public PlayersControllerTests()
        {
            controller = new PlayersController(mockDb.Db);
        }

        protected MockLeaderboardsContext mockDb = new MockLeaderboardsContext();
        protected PlayersController controller;

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var db = new LeaderboardsContext();

                // Act
                var controller = new PlayersController(db);

                // Assert
                Assert.IsAssignableFrom<PlayersController>(controller);
            }
        }

        public class GetPlayersMethod : PlayersControllerTests
        {
            public GetPlayersMethod()
            {
                mockDb.Players.AddRange(Players);
            }

            protected PlayersPagination pagination = new PlayersPagination();
            protected PlayersSortParams sort = new PlayersSortParams();

            [Fact]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.Limit = 2;

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "1", "2" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                Assert.Equal(expectedIds, actualIds);
            }

            [Fact]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                pagination.Offset = 2;

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "3", "4", "5" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                Assert.Equal(expectedIds, actualIds);
            }

            [Fact]
            public async Task SortIsSpecified_ReturnsSortedResults()
            {
                // Arrange
                sort.Add("-id");

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "5", "4", "3", "2", "1" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                Assert.Equal(expectedIds, actualIds);
            }

            [Fact]
            public async Task QIsSpecified_ReturnsFilteredPlayers()
            {
                // Arrange
                var q = "Sta";

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort, q);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                var expectedIds = new[] { "5" };
                var actualIds = playerDTOs.Select(p => p.Id).ToList();
                Assert.Equal(expectedIds, actualIds);
            }

            [Fact]
            public async Task QIsNotSpecified_ReturnsPlayers()
            {
                // Arrange -> Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var playerDTOs = contentResult.Content.Players;
                Assert.Equal(5, playerDTOs.Count());
            }
        }

        public class GetPlayerMethod : PlayersControllerTests
        {
            protected long steamId;

            [Fact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayer(steamId);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [Fact]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                steamId = 76561197960481221;
                mockDb.Players.Add(new Player { SteamId = steamId });

                // Act
                var result = await controller.GetPlayer(steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayerEnvelope>>(result);
            }
        }

        public class GetPlayerEntriesMethod : PlayersControllerTests
        {
            protected long steamId;
            protected LeaderboardIdParams lbids = new LeaderboardIdParams();
            protected Products products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

            [Fact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayerEntries(steamId, lbids, products);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [Fact]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                steamId = 76561197960481221;
                mockDb.Players.Add(new Player { SteamId = steamId });

                // Act
                var result = await controller.GetPlayerEntries(steamId, lbids, products);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayerEntriesDTO>>(result);
            }
        }

        public class GetPlayerEntryMethod : PlayersControllerTests
        {
            protected int lbid;
            protected int steamId;

            [Fact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [Fact]
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
                    Replay = new Replay(),
                };
                mockDb.Entries.Add(entry);

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EntryDTO>>(result);
            }

            [Fact]
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
                    Replay = new Replay
                    {
                        ReplayId = 234,
                        Version = 74,
                        KilledBy = "BOMB",
                    },
                };
                mockDb.Entries.Add(entry);

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EntryDTO>>(result);
                var contentResult = (OkNegotiatedContentResult<EntryDTO>)result;
                var content = contentResult.Content;
                Assert.Equal(74, content.Version);
                Assert.Equal("BOMB", content.KilledBy);
            }
        }

        public class GetPlayerDailyEntriesMethod : PlayersControllerTests
        {
            protected long steamId;
            protected LeaderboardIdParams leaderboardIds = new LeaderboardIdParams();
            protected Products products = new Products(LeaderboardCategories.Products.Select(p => p.Name));

            [Fact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, leaderboardIds, products);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [Fact]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                var steamId = 76561197960481221;
                mockDb.Players.Add(new Player { SteamId = steamId });

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, leaderboardIds, products);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayerDailyEntriesDTO>>(result);
            }
        }

        public class GetPlayerDailyEntryMethod : PlayersControllerTests
        {
            protected int lbid;
            protected long steamId;

            [Fact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                lbid = 234265;
                steamId = 1;

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [Fact]
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
                    Replay = new Replay(),
                };
                mockDb.DailyEntries.Add(entry);

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyEntryDTO>>(result);
            }

            [Fact]
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
                    Replay = new Replay
                    {
                        ReplayId = 234,
                        Version = 74,
                        KilledBy = "BOMB",
                    },
                };
                mockDb.DailyEntries.Add(entry);

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyEntryDTO>>(result);
                var contentResult = (OkNegotiatedContentResult<DailyEntryDTO>)result;
                var content = contentResult.Content;
                Assert.Equal(74, content.Version);
                Assert.Equal("BOMB", content.KilledBy);
            }
        }

        public class DisposeMethod : PlayersControllerTests
        {
            [Fact]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.MockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [Fact]
            public void DisposesMoreThanOnce_OnlyDisposesDbOnce()
            {
                // Arrange -> Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.MockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            public IntegrationTests(IntegrationTestsFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task GetPlayers()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players?q=m");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayers);
            }

            [Fact]
            public async Task GetPlayer()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198036838485");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayer);
            }

            [Fact]
            public async Task GetPlayerEntries()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561197976996341/entries");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerEntries);
            }

            [Fact]
            public async Task GetPlayerEntriesFilteredByLbids()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561197999613276/entries?lbids=739999,2047515");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerEntriesFilteredByLbids);
            }

            [Fact]
            public async Task GetPlayerEntry()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561197999613276/entries/739999");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerEntry);
            }

            [Fact]
            public async Task GetPlayerDailyEntries()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198044686391/entries/dailies");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerDailyEntries);
            }

            [Fact]
            public async Task GetPlayerDailyEntriesFilteredByLbids()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198044686391/entries/dailies?lbids=742742,1705585");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerDailyEntriesFilteredByLbids);
            }

            [Fact]
            public async Task GetPlayerDailyEntry()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198044686391/entries/dailies/742742");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerDailyEntry);
            }
        }
    }
}
