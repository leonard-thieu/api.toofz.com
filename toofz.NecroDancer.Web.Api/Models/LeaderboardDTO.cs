using System;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer leaderboard.
    /// </summary>
    [DataContract]
    public sealed class LeaderboardDTO
    {
        /// <summary>
        /// Steam lbid
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product (e.g. classic, amplified)
        /// </summary>
        [DataMember(Name = "product")]
        public string Product { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer character (e.g. all-characters, cadence, story-mode)
        /// </summary>
        [DataMember(Name = "character")]
        public string Character { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer mode (e.g. standard, hard, no-return)
        /// </summary>
        [DataMember(Name = "mode")]
        public string Mode { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer run (e.g. score, seeded-speed, deathless)
        /// </summary>
        [DataMember(Name = "run")]
        public string Run { get; set; }
        /// <summary>
        /// Display name for the leaderboard.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Total number of entries
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
    }
}