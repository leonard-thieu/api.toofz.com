namespace toofz.NecroDancer.Web.Api.Models
{
    /// <summary>
    /// Represents the response of a bulk store operation.
    /// </summary>
    public sealed class BulkStore
    {
        /// <summary>
        /// The number of rows affected.
        /// </summary>
        public int rows_affected { get; set; }
    }
}