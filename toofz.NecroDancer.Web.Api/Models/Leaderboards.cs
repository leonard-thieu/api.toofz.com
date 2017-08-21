using System.Collections.Generic;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer leaderboards.
    /// </summary>
    public sealed class Leaderboards
    {
        /// <summary>
        /// Total number of leaderboards.
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// A collection of leaderboards.
        /// </summary>
        public IEnumerable<Leaderboard> leaderboards { get; set; }
    }
}