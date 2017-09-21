using System.Runtime.Serialization;

namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// Represents the response of a bulk store operation.
    /// </summary>
    [DataContract]
    public sealed class BulkStoreDTO
    {
        /// <summary>
        /// The number of rows affected.
        /// </summary>
        [DataMember(Name = "rows_affected")]
        public int RowsAffected { get; set; }
    }
}