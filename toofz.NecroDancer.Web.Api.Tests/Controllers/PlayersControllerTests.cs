using System.Linq;
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
    public class PlayersControllerTests
    {
        public PlayersControllerTests(MockDatabaseFixture fixture)
        {
            mockDb = fixture.CreateMockLeaderboardsContext();
            controller = new PlayersController(mockDb.Object);
        }

        private readonly Mock<ILeaderboardsContext> mockDb;
        private readonly PlayersController controller;

        public class Constructor
        {
            [DisplayFact]
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
            public GetPlayersMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly PlayersPagination pagination = new PlayersPagination();
            private readonly PlayersSortParams sort = new PlayersSortParams();

            [DisplayFact]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.Limit = 2;

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var players = contentResult.Content.Players;
                Assert.Equal(new[]
                {
                    "76561198044686391",
                    "76561198070596881",
                }, players.Select(p => p.Id));
            }

            [DisplayFact]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                pagination.Offset = 2;

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var players = contentResult.Content.Players;
                Assert.Equal(new[]
                {
                    "76561198063289187", "76561197970345045", "76561198060310297", "76561198089268754", "76561198042211886",
                    "76561198066366334", "76561198019090079", "76561198043152248", "76561198051520646", "76561198009941176",
                    "76561198053569604", "76561198067365023", "76561198165150163", "76561197965804468", "76561198165350197",
                    "76561198094923763", "76561198014525041", "76561198133195362", "76561198052229724", "76561198097897774",
                    "76561198044778237", "76561198051380610", "76561198154717415", "76561198077792175", "76561198025119291",
                    "76561198081127587", "76561197996651110", "76561197983885363", "76561198119176120", "76561198151496151",
                    "76561198141270473", "76561197995016888", "76561198059437934", "76561198049106029", "76561198055894548",
                    "76561198171801786", "76561198025462847", "76561198190560728", "76561197985764354", "76561198045260150",
                    "76561197993680740", "76561198079345283", "76561198116399814", "76561197965776309", "76561198038077223",
                    "76561198141006932", "76561198062534705", "76561197999613276", "76561198024988025", "76561198079361434",
                    "76561197976996341", "76561198054034534", "76561197977073983", "76561198096055448", "76561198236008636",
                    "76561197960799577", "76561197993693567", "76561198004598424", "76561198044677675", "76561198000048416",
                    "76561198097906515", "76561198078776790", "76561198067116391", "76561198197721531", "76561198163898899",
                    "76561198057233671", "76561198283190045", "76561198122003545", "76561198036838485", "76561198027716191",
                    "76561198020400998", "76561198045544962", "76561198033353739", "76561198025231301", "76561197997799915",
                    "76561198286249466", "76561197970885886", "76561197978294223", "76561198020096110", "76561198128729097",
                    "76561198013056914", "76561198048052937", "76561198100455018", "76561198093336562", "76561198053984875",
                    "76561198260704537", "76561198162674123", "76561198041896308", "76561198101702277", "76561198095968951",
                    "76561197966692309", "76561197999121403", "76561198004551968", "76561198017976778", "76561197987591887",
                    "76561198007439754", "76561198106146121", "76561198052465462",
                }, players.Select(p => p.Id));
            }

            [DisplayFact]
            public async Task SortIsSpecified_ReturnsSortedResults()
            {
                // Arrange
                sort.Add("-id");

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var players = contentResult.Content.Players;
                Assert.Equal(new[]
                {
                    "76561198286249466", "76561198283190045", "76561198260704537", "76561198236008636", "76561198197721531",
                    "76561198190560728", "76561198171801786", "76561198165350197", "76561198165150163", "76561198163898899",
                    "76561198162674123", "76561198154717415", "76561198151496151", "76561198141270473", "76561198141006932",
                    "76561198133195362", "76561198128729097", "76561198122003545", "76561198119176120", "76561198116399814",
                    "76561198106146121", "76561198101702277", "76561198100455018", "76561198097906515", "76561198097897774",
                    "76561198096055448", "76561198095968951", "76561198094923763", "76561198093336562", "76561198089268754",
                    "76561198081127587", "76561198079361434", "76561198079345283", "76561198078776790", "76561198077792175",
                    "76561198070596881", "76561198067365023", "76561198067116391", "76561198066366334", "76561198063289187",
                    "76561198062534705", "76561198060310297", "76561198059437934", "76561198057233671", "76561198055894548",
                    "76561198054034534", "76561198053984875", "76561198053569604", "76561198052465462", "76561198052229724",
                    "76561198051520646", "76561198051380610", "76561198049106029", "76561198048052937", "76561198045544962",
                    "76561198045260150", "76561198044778237", "76561198044686391", "76561198044677675", "76561198043152248",
                    "76561198042211886", "76561198041896308", "76561198038077223", "76561198036838485", "76561198033353739",
                    "76561198027716191", "76561198025462847", "76561198025231301", "76561198025119291", "76561198024988025",
                    "76561198020400998", "76561198020096110", "76561198019090079", "76561198017976778", "76561198014525041",
                    "76561198013056914", "76561198009941176", "76561198007439754", "76561198004598424", "76561198004551968",
                    "76561198000048416", "76561197999613276", "76561197999121403", "76561197997799915", "76561197996651110",
                    "76561197995016888", "76561197993693567", "76561197993680740", "76561197987591887", "76561197985764354",
                    "76561197983885363", "76561197978294223", "76561197977073983", "76561197976996341", "76561197970885886",
                    "76561197970345045", "76561197966692309", "76561197965804468", "76561197965776309", "76561197960799577",
                }, players.Select(p => p.Id));
            }

            [DisplayFact]
            public async Task QIsSpecified_ReturnsFilteredPlayers()
            {
                // Arrange
                var q = "s";

                // Act
                var actionResult = await controller.GetPlayers(pagination, sort, q);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var players = contentResult.Content.Players;
                Assert.Equal(new[]
                {
                    "76561198042211886", "76561198094923763", "76561198067116391", "76561198122003545", "76561198106146121"
                }, players.Select(p => p.Id));
            }

            [DisplayFact]
            public async Task QIsNotSpecified_ReturnsPlayers()
            {
                // Arrange -> Act
                var actionResult = await controller.GetPlayers(pagination, sort);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayersEnvelope>>(actionResult);
                var contentResult = (OkNegotiatedContentResult<PlayersEnvelope>)actionResult;
                var players = contentResult.Content.Players;
                Assert.Equal(100, players.Count());
            }
        }

        public class GetPlayerMethod : PlayersControllerTests
        {
            public GetPlayerMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private long steamId;

            [DisplayFact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayer(steamId);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [DisplayFact]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                steamId = 76561198097897774;

                // Act
                var result = await controller.GetPlayer(steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayerEnvelope>>(result);
            }
        }

        public class GetPlayerEntriesMethod : PlayersControllerTests
        {
            public GetPlayerEntriesMethod(MockDatabaseFixture fixture) : base(fixture)
            {
                products = new Products(fixture.Products.Select(p => p.Name));
            }

            private long steamId;
            private readonly LeaderboardIdParams lbids = new LeaderboardIdParams();
            private readonly Products products;

            [DisplayFact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayerEntries(steamId, lbids, products);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [DisplayFact]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                steamId = 76561198052229724;

                // Act
                var result = await controller.GetPlayerEntries(steamId, lbids, products);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayerEntriesDTO>>(result);
            }
        }

        public class GetPlayerEntryMethod : PlayersControllerTests
        {
            public GetPlayerEntryMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private int lbid;
            private long steamId;

            [DisplayFact]
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

            [DisplayFact]
            public async Task ReturnsPlayerEntry()
            {
                // Arrange
                lbid = 742526;
                steamId = 76561198097897774;

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EntryDTO>>(result);
            }

            [DisplayFact]
            public async Task ReplayFound_AddsReplayInformation()
            {
                // Arrange
                lbid = 740000;
                steamId = 76561198078776790;

                // Act
                var result = await controller.GetPlayerEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<EntryDTO>>(result);
                var contentResult = (OkNegotiatedContentResult<EntryDTO>)result;
                var content = contentResult.Content;
                Assert.Equal(75, content.Version);
                Assert.Equal("BLOOD MAGIC", content.KilledBy);
            }
        }

        public class GetPlayerDailyEntriesMethod : PlayersControllerTests
        {
            public GetPlayerDailyEntriesMethod(MockDatabaseFixture fixture) : base(fixture)
            {
                products = new Products(fixture.Products.Select(p => p.Name));
            }

            private long steamId;
            private readonly LeaderboardIdParams leaderboardIds = new LeaderboardIdParams();
            private readonly Products products;

            [DisplayFact]
            public async Task PlayerNotFound_ReturnsNotFound()
            {
                // Arrange
                steamId = 1;

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, leaderboardIds, products);

                // Assert
                Assert.IsAssignableFrom<NotFoundResult>(result);
            }

            [DisplayFact]
            public async Task ReturnsPlayerEntries()
            {
                // Arrange
                var steamId = 76561198063289187;

                // Act
                var result = await controller.GetPlayerDailyEntries(steamId, leaderboardIds, products);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<PlayerDailyEntriesDTO>>(result);
            }
        }

        public class GetPlayerDailyEntryMethod : PlayersControllerTests
        {
            public GetPlayerDailyEntryMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private int lbid;
            private long steamId;

            [DisplayFact]
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

            [DisplayFact]
            public async Task ReturnsPlayerEntry()
            {
                // Arrange
                lbid = 1751947;
                steamId = 76561198025462847;

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyEntryDTO>>(result);
            }

            [DisplayFact]
            public async Task ReplayFound_AddsReplayInformation()
            {
                // Arrange
                lbid = 628864;
                steamId = 76561198097906515;

                // Act
                var result = await controller.GetPlayerDailyEntry(lbid, steamId);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<DailyEntryDTO>>(result);
                var contentResult = (OkNegotiatedContentResult<DailyEntryDTO>)result;
                var content = contentResult.Content;
                Assert.Equal(44, content.Version);
                Assert.Equal("BLACK SKELETON", content.KilledBy);
            }
        }

        public class DisposeMethod : PlayersControllerTests
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
            public async Task GetPlayers()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players?q=m");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayers);
            }

            [DisplayFact]
            public async Task GetPlayer()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198036838485");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayer);
            }

            [DisplayFact]
            public async Task GetPlayerEntries()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561197976996341/entries");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerEntries);
            }

            [DisplayFact]
            public async Task GetPlayerEntriesFilteredByLbids()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561197999613276/entries?lbids=739999,2047515");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerEntriesFilteredByLbids);
            }

            [DisplayFact]
            public async Task GetPlayerEntry()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561197999613276/entries/739999");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerEntry);
            }

            [DisplayFact]
            public async Task GetPlayerDailyEntries()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198044686391/entries/dailies");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerDailyEntries);
            }

            [DisplayFact]
            public async Task GetPlayerDailyEntriesFilteredByLbids()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/players/76561198044686391/entries/dailies?lbids=742742,1705585");

                // Assert
                await RespondsWithAsync(response, Resources.GetPlayerDailyEntriesFilteredByLbids);
            }

            [DisplayFact]
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
