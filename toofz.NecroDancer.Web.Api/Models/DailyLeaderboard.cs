using System;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer daily leaderboard.
    /// </summary>
    public sealed class DailyLeaderboard
    {
        /// <summary>
        /// Steam lbid
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// The date of the daily leaderboard.
        /// </summary>
        public DateTime date { get; set; }
        /// <summary>
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        public DateTime updated_at { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product (e.g. classic, amplified)
        /// </summary>
        public string product { get; set; }
        /// <summary>
        /// Indicates if the daily leaderboard is a production leaderboard.
        /// </summary>
        public bool production { get; set; }
    }
}