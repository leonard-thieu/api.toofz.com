using System.Collections.Generic;
using Moq;
using toofz.NecroDancer.Leaderboards;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests
{
    class MockLeaderboardsContext : Mock<ILeaderboardsContext>
    {
        public MockLeaderboardsContext()
        {
            MockLeaderboards = new MockDbSet<Leaderboard>(Leaderboards);
            MockDb.Setup(d => d.Leaderboards).Returns(MockLeaderboards.Object);
            MockEntries = new MockDbSet<Entry>(Entries);
            MockDb.Setup(d => d.Entries).Returns(MockEntries.Object);
            MockDailyLeaderboards = new MockDbSet<DailyLeaderboard>(DailyLeaderboards);
            MockDb.Setup(d => d.DailyLeaderboards).Returns(MockDailyLeaderboards.Object);
            MockDailyEntries = new MockDbSet<DailyEntry>(DailyEntries);
            MockDb.Setup(d => d.DailyEntries).Returns(MockDailyEntries.Object);
            MockPlayers = new MockDbSet<Player>(Players);
            MockDb.Setup(d => d.Players).Returns(MockPlayers.Object);
            MockReplays = new MockDbSet<Replay>(Replays);
            MockDb.Setup(d => d.Replays).Returns(MockReplays.Object);
            Db = MockDb.Object;
        }

        public MockDbSet<Leaderboard> MockLeaderboards { get; set; }
        public List<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
        public MockDbSet<Entry> MockEntries { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
        public MockDbSet<DailyLeaderboard> MockDailyLeaderboards { get; set; }
        public List<DailyLeaderboard> DailyLeaderboards { get; set; } = new List<DailyLeaderboard>();
        public MockDbSet<DailyEntry> MockDailyEntries { get; set; }
        public List<DailyEntry> DailyEntries { get; set; } = new List<DailyEntry>();
        public MockDbSet<Player> MockPlayers { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public MockDbSet<Replay> MockReplays { get; set; }
        public List<Replay> Replays { get; set; } = new List<Replay>();
        public Mock<ILeaderboardsContext> MockDb { get; set; } = new Mock<ILeaderboardsContext>();
        public ILeaderboardsContext Db { get; set; }
    }
}
