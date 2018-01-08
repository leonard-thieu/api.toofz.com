using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Steam players.
    /// </summary>
    [DataContract]
    public sealed class PlayersEnvelope
    {
        /// <summary>
        /// Total number of players.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of players.
        /// </summary>
        [DataMember(Name = "players")]
        public IEnumerable<PlayerDTO> Players { get; set; }
    }
}