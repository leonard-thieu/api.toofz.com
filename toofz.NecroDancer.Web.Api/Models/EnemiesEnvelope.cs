using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// A page of Crypt of the NecroDancer enemies.
    /// </summary>
    [DataContract]
    public sealed class EnemiesEnvelope
    {
        /// <summary>
        /// The total number of enemies in the result set.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>
        /// A collection of enemies.
        /// </summary>
        [DataMember(Name = "enemies")]
        public IEnumerable<EnemyDTO> Enemies { get; set; }
    }
}