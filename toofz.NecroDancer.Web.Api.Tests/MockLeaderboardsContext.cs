using System.Collections.Generic;
using Moq;
using toofz.NecroDancer.Leaderboards;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class MockLeaderboardsContext : Mock<ILeaderboardsContext>
    {
        public MockLeaderboardsContext()
        {
            MockDb.Setup(d => d.Leaderboards).Returns(new FakeDbSet<Leaderboard>(Leaderboards));
            MockDb.Setup(d => d.Entries).Returns(new FakeDbSet<Entry>(Entries));
            MockDb.Setup(d => d.DailyLeaderboards).Returns(new FakeDbSet<DailyLeaderboard>(DailyLeaderboards));
            MockDb.Setup(d => d.DailyEntries).Returns(new FakeDbSet<DailyEntry>(DailyEntries));
            MockDb.Setup(d => d.Players).Returns(new FakeDbSet<Player>(Players));
            MockDb.Setup(d => d.Replays).Returns(new FakeDbSet<Replay>(Replays));
            Db = MockDb.Object;
        }

        public List<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();
        public List<Entry> Entries { get; set; } = new List<Entry>();
        public List<DailyLeaderboard> DailyLeaderboards { get; set; } = new List<DailyLeaderboard>();
        public List<DailyEntry> DailyEntries { get; set; } = new List<DailyEntry>();
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Replay> Replays { get; set; } = new List<Replay>();
        public Mock<ILeaderboardsContext> MockDb { get; set; } = new Mock<ILeaderboardsContext>();
        public ILeaderboardsContext Db { get; set; }
    }
}
