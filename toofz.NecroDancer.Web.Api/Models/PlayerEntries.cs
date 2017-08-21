using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    public sealed class PlayerEntries
    {
        /// <summary>
        /// The Steam player.
        /// </summary>
        public Player player { get; set; }
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