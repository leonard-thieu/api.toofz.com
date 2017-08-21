using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer leaderboard entries.
    /// </summary>
    public sealed class LeaderboardEntries
    {
        /// <summary>
        /// The Crypt of the NecroDancer leaderboard.
        /// </summary>
        public Leaderboard leaderboard { get; set; }
        /// <summary>
        /// Total number of leaderboard entries.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of leaderboard entries.
        /// </summary>
        public IEnumerable<Entry> entries { get; set; }
    }
}