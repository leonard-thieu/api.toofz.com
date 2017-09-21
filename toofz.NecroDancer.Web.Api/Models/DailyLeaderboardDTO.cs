using System;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Crypt of the NecroDancer daily leaderboard.
    /// </summary>
    [DataContract]
    public sealed class DailyLeaderboardDTO
    {
        /// <summary>
        /// Steam lbid
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// The date of the daily leaderboard.
        /// </summary>
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }
        /// <summary>
        /// Time that the leaderboard was last updated at (in UTC)
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Crypt of the NecroDancer product (e.g. classic, amplified)
        /// </summary>
        [DataMember(Name = "product")]
        public string Product { get; set; }
        /// <summary>
        /// Indicates if the daily leaderboard is a production leaderboard.
        /// </summary>
        [DataMember(Name = "production")]
        public bool IsProduction { get; set; }
    }
}