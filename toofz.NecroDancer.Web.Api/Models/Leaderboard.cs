using System;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer leaderboard.
    /// </summary>
    public sealed class Leaderboard
    {
        /// <summary>
        /// Steam lbid
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product (e.g. classic, amplified)
        /// </summary>
        public string product { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer character (e.g. all-characters, cadence, story-mode)
        /// </summary>
        public string character { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer mode (e.g. standard, hard, no-return)
        /// </summary>
        public string mode { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer run (e.g. score, seeded-speed, deathless)
        /// </summary>
        public string run { get; set; }
        /// <summary>
        /// Display name for the leaderboard.
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        public DateTime updated_at { get; set; }
        /// <summary>
        /// Total number of entries
        /// </summary>
        public int total { get; set; }
    }
}