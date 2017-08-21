using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer daily leaderboards.
    /// </summary>
    public sealed class DailyLeaderboards
    {
        /// <summary>
        /// Total number of daily leaderboards.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of daily leaderboards.
        /// </summary>
        public IEnumerable<DailyLeaderboard> leaderboards { get; set; }
    }
}