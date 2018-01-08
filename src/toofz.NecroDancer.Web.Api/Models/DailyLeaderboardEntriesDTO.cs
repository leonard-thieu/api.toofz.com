using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer daily leaderboard entries.
    /// </summary>
    [DataContract]
    public sealed class DailyLeaderboardEntriesDTO
    {
        /// <summary>
        /// The Crypt of the NecroDancer daily leaderboard.
        /// </summary>
        [DataMember(Name = "leaderboard")]
        public DailyLeaderboardDTO Leaderboard { get; set; }
        /// <summary>
        /// Total number of daily leaderboard entries.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of daily leaderboard entries.
        /// </summary>
        [DataMember(Name = "entries")]
        public IEnumerable<EntryDTO> Entries { get; set; }
    }
}