using System.Collections.Generic;
using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    [DataContract]
    public sealed class ReplaysEnvelope
    {
        [DataMember(Name = "total")]
        public int Total { get; set; }
        [DataMember(Name = "replays")]
        public IEnumerable<ReplayDTO> Replays { get; set; }
    }
}