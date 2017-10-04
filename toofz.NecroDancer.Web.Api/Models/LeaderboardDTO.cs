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
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Display name for the leaderboard.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// Indicates if the leaderboard is a production leaderboard.
        /// </summary>
        [DataMember(Name = "production")]
        public bool IsProduction { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product name (e.g. classic, amplified)
        /// </summary>
        [DataMember(Name = "product")]
        public string ProductName { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product
        /// </summary>
        [DataMember(Name = "_product")]
        public ProductDTO Product { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer mode name (e.g. standard, hard, no-return)
        /// </summary>
        [DataMember(Name = "mode")]
        public string ModeName { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer mode
        /// </summary>
        [DataMember(Name = "_mode")]
        public ModeDTO Mode { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer run name (e.g. score, seeded-speed, deathless)
        /// </summary>
        [DataMember(Name = "run")]
        public string RunName { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer run
        /// </summary>
        [DataMember(Name = "_run")]
        public RunDTO Run { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer character name (e.g. all-characters, cadence, story-mode)
        /// </summary>
        [DataMember(Name = "character")]
        public string CharacterName { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer character
        /// </summary>
        [DataMember(Name = "_character")]
        public CharacterDTO Character { get; set; }
        /// <summary>
        /// Total number of entries
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
    }
}