using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer daily leaderboard entries.
    /// </summary>
    public sealed class DailyLeaderboardEntries
    {
        /// <summary>
        /// The Crypt of the NecroDancer daily leaderboard.
        /// </summary>
        public DailyLeaderboard leaderboard { get; set; }
        /// <summary>
        /// Total number of daily leaderboard entries.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of daily leaderboard entries.
        /// </summary>
        public IEnumerable<Entry> entries { get; set; }
    }
}