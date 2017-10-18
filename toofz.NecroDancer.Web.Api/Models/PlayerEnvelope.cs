using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    [DataContract]
    public sealed class PlayerEnvelope
    {
        /// <summary>
        /// The Steam player.
        /// </summary>
        [DataMember(Name = "player")]
        public PlayerDTO Player { get; set; }
    }
}