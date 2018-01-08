using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    [DataContract]
    public sealed class PlayerEntriesDTO
    {
        /// <summary>
        /// The Steam player.
        /// </summary>
        [DataMember(Name = "player")]
        public PlayerDTO Player { get; set; }
        /// <summary>
        /// Total number of leaderboard entries.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of leaderboard entries.
        /// </summary>
        [DataMember(Name = "entries")]
        public IEnumerable<EntryDTO> Entries { get; set; }
    }
}