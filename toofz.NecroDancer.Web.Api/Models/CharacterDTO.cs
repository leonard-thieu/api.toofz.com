using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    [DataContract]
    public sealed class CharacterDTO
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
    }
}