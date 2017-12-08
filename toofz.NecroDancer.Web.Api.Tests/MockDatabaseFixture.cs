using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using toofz.Data;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MockDatabaseFixture
    {
        public MockDatabaseFixture()
        {
            FixupLeaderboardsContextEntities();
        }

        public readonly IEnumerable<Data.Item> Items = JsonConvert.DeserializeObject<IEnumerable<Data.Item>>(NecroDancerResources.Items);
        public readonly IEnumerable<Data.Enemy> Enemies = JsonConvert.DeserializeObject<IEnumerable<Data.Enemy>>(NecroDancerResources.Enemies);

        public readonly IEnumerable<Product> Products = JsonConvert.DeserializeObject<IEnumerable<Product>>(LeaderboardsResources.Products);
        public readonly IEnumerable<Mode> Modes = JsonConvert.DeserializeObject<IEnumerable<Mode>>(LeaderboardsResources.Modes);
        public readonly IEnumerable<Run> Runs = JsonConvert.DeserializeObject<IEnumerable<Run>>(LeaderboardsResources.Runs);
        public readonly IEnumerable<Character> Characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(LeaderboardsResources.Characters);
        public readonly IEnumerable<Leaderboard> Leaderboards = JsonConvert.DeserializeObject<IEnumerable<Leaderboard>>(LeaderboardsResources.Leaderboards);
        public readonly IEnumerable<Entry> Entries = JsonConvert.DeserializeObject<IEnumerable<Entry>>(LeaderboardsResources.Entries);
        public readonly IEnumerable<DailyLeaderboard> DailyLeaderboards = JsonConvert.DeserializeObject<IEnumerable<DailyLeaderboard>>(LeaderboardsResources.DailyLeaderboards);
        public readonly IEnumerable<DailyEntry> DailyEntries = JsonConvert.DeserializeObject<IEnumerable<DailyEntry>>(LeaderboardsResources.DailyEntries);
        public readonly IEnumerable<Player> Players = JsonConvert.DeserializeObject<IEnumerable<Player>>(LeaderboardsResources.Players);
        public readonly IEnumerable<Replay> Replays = JsonConvert.DeserializeObject<IEnumerable<Replay>>(LeaderboardsResources.Replays);

        private void FixupLeaderboardsContextEntities()
        {
            foreach (var leaderboard in Leaderboards)
            {
                leaderboard.Product = Products.First(p => p.ProductId == leaderboard.ProductId);
                leaderboard.Mode = Modes.First(m => m.ModeId == leaderboard.ModeId);
                leaderboard.Run = Runs.First(r => r.RunId == leaderboard.RunId);
                leaderboard.Character = Characters.First(c => c.CharacterId == leaderboard.CharacterId);
            }

            Entries.Join(Leaderboards, e => e.LeaderboardId, l => l.LeaderboardId, (e, l) =>
            {
                e.Leaderboard = l;

                return e;
            }).Count();
            Entries.Join(Players, e => e.SteamId, p => p.SteamId, (e, p) =>
            {
                e.Player = p;

                return e;
            }).Count();
            Entries.Join(Replays, e => e.ReplayId, r => r.ReplayId, (e, r) =>
            {
                e.Replay = r;

                return e;
            }).Count();
            // TODO: This is a bit hacky. Null entities should just return null for its properties in queries.
            //       Can this be fixed in FakeDbSet?
            foreach (var entry in Entries.Where(e => e.ReplayId == null))
            {
                entry.Replay = new Replay();
            }

            foreach (var dailyLeaderboard in DailyLeaderboards)
            {
                dailyLeaderboard.Product = Products.First(p => p.ProductId == dailyLeaderboard.ProductId);
            }

            DailyEntries.Join(DailyLeaderboards, e => e.LeaderboardId, l => l.LeaderboardId, (e, l) =>
            {
                e.Leaderboard = l;

                return e;
            }).Count();
            DailyEntries.Join(Players, e => e.SteamId, p => p.SteamId, (e, p) =>
            {
                e.Player = p;

                return e;
            }).Count();
            DailyEntries.Join(Replays, e => e.ReplayId, r => r.ReplayId, (e, r) =>
            {
                e.Replay = r;

                return e;
            }).Count();
            foreach (var dailyEntry in DailyEntries.Where(e => e.ReplayId == null))
            {
                dailyEntry.Replay = new Replay();
            }
        }

        public Mock<INecroDancerContext> CreateMockNecroDancerContext()
        {
            var mockDb = new Mock<INecroDancerContext>();

            var dbItems = new FakeDbSet<Data.Item>(Items);
            mockDb.Setup(x => x.Items).Returns(dbItems);

            var dbEnemies = new FakeDbSet<Data.Enemy>(Enemies);
            mockDb.Setup(x => x.Enemies).Returns(dbEnemies);

            return mockDb;
        }

        public Mock<ILeaderboardsContext> CreateMockLeaderboardsContext()
        {
            var mockDb = new Mock<ILeaderboardsContext>();

            var dbProducts = new FakeDbSet<Product>(Products);
            mockDb.Setup(d => d.Products).Returns(dbProducts);

            var dbModes = new FakeDbSet<Mode>(Modes);
            mockDb.Setup(d => d.Modes).Returns(dbModes);

            var dbRuns = new FakeDbSet<Run>(Runs);
            mockDb.Setup(d => d.Runs).Returns(dbRuns);

            var dbCharacters = new FakeDbSet<Character>(Characters);
            mockDb.Setup(d => d.Characters).Returns(dbCharacters);

            var dbLeaderboards = new FakeDbSet<Leaderboard>(Leaderboards);
            mockDb.Setup(d => d.Leaderboards).Returns(dbLeaderboards);

            var dbEntries = new FakeDbSet<Entry>(Entries);
            mockDb.Setup(d => d.Entries).Returns(dbEntries);

            var dbDailyLeaderboards = new FakeDbSet<DailyLeaderboard>(DailyLeaderboards);
            mockDb.Setup(d => d.DailyLeaderboards).Returns(dbDailyLeaderboards);

            var dbDailyEntries = new FakeDbSet<DailyEntry>(DailyEntries);
            mockDb.Setup(d => d.DailyEntries).Returns(dbDailyEntries);

            var dbPlayers = new FakeDbSet<Player>(Players);
            mockDb.Setup(d => d.Players).Returns(dbPlayers);

            var dbReplays = new FakeDbSet<Replay>(Replays);
            mockDb.Setup(d => d.Replays).Returns(dbReplays);

            return mockDb;
        }
    }

    [CollectionDefinition(Name)]
    public class MockDatabaseCollection : ICollectionFixture<MockDatabaseFixture>
    {
        public const string Name = "Mock database collection";
    }
}
