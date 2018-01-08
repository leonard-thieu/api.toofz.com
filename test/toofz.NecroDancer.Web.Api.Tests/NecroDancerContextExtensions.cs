using toofz.Data;
using toofz.NecroDancer.Web.Api.Tests.Properties;

namespace toofz.NecroDancer.Web.Api.Tests
{
    internal static class NecroDancerContextExtensions
    {
        public static void EnsureSeedData(this NecroDancerContext context)
        {
            context.Items.AddRange(Util.ReadJsonArray<Data.Item>(NecroDancerResources.Items));
            context.Enemies.AddRange(Util.ReadJsonArray<Data.Enemy>(NecroDancerResources.Enemies));

            context.Products.AddRange(Util.ReadJsonArray<Product>(LeaderboardsResources.Products));
            context.Modes.AddRange(Util.ReadJsonArray<Mode>(LeaderboardsResources.Modes));
            context.Runs.AddRange(Util.ReadJsonArray<Run>(LeaderboardsResources.Runs));
            context.Characters.AddRange(Util.ReadJsonArray<Character>(LeaderboardsResources.Characters));
            context.Leaderboards.AddRange(Util.ReadJsonArray<Leaderboard>(LeaderboardsResources.Leaderboards));
            context.Entries.AddRange(Util.ReadJsonArray<Entry>(LeaderboardsResources.Entries));
            context.DailyLeaderboards.AddRange(Util.ReadJsonArray<DailyLeaderboard>(LeaderboardsResources.DailyLeaderboards));
            context.DailyEntries.AddRange(Util.ReadJsonArray<DailyEntry>(LeaderboardsResources.DailyEntries));
            context.Players.AddRange(Util.ReadJsonArray<Player>(LeaderboardsResources.Players));
            context.Replays.AddRange(Util.ReadJsonArray<Replay>(LeaderboardsResources.Replays));

            context.SaveChanges();
        }
    }
}
