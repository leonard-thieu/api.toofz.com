using System;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A Steam player.
    /// </summary>
    [DataContract]
    public sealed class PlayerDTO
    {
        /// <summary>
        /// The player's Steam ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
        /// <summary>
        /// The time (in UTC) that the player's data was retrieved at.
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// The player's display name.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// The URL of the player's avatar.
        /// </summary>
        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }
    }
}