using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    /// <summary>
    /// Methods for working with Crypt of the NecroDancer replays.
    /// </summary>
    [RoutePrefix("replays")]
    public sealed class ReplaysController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaysController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        /// <param name="storeClient">The store client used to store submitted data.</param>
        public ReplaysController(
            ILeaderboardsContext db,
            ILeaderboardsStoreClient storeClient)
        {
            this.db = db;
            this.storeClient = storeClient;
        }

        private readonly ILeaderboardsContext db;
        private readonly ILeaderboardsStoreClient storeClient;

        [ResponseType(typeof(ReplaysEnvelope))]
        [Route("")]
        public async Task<IHttpActionResult> GetReplays(
            ReplaysPagination pagination,
            int? version = null,
            int? error = null,
            CancellationToken cancellationToken = default)
        {
            var query = from r in db.Replays.AsNoTracking()
                        where r.Version == version && r.ErrorCode == error
                        orderby r.ReplayId
                        select r;

            var total = await query.CountAsync(cancellationToken);
            var replays = await (from r in query
                                 select new ReplayDTO
                                 {
                                     Id = r.ReplayId.ToString(),
                                     Error = r.ErrorCode,
                                     Seed = r.Seed,
                                     Version = r.Version,
                                     KilledBy = r.KilledBy,
                                 })
                                 .Page(pagination)
                                 .ToListAsync(cancellationToken);

            var content = new ReplaysEnvelope
            {
                Total = total,
                Replays = replays,
            };

            return Ok(content);
        }

        /// <summary>
        /// Updates replays.
        /// </summary>
        /// <param name="replays">A list of replays.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns the number of replays updated.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.BadRequest">
        /// Any replays failed validation.
        /// </httpStatusCode>
        /// <httpStatusCode cref="HttpStatusCode.Conflict">
        /// There are duplicate IDs.
        /// </httpStatusCode>
        [ResponseType(typeof(BulkStoreDTO))]
        [Route("")]
        [Authorize(Users = "ReplaysService")]
        public async Task<IHttpActionResult> PostReplays(
            IEnumerable<ReplayModel> replays,
            CancellationToken cancellationToken = default)
        {
            var model = (from r in replays
                         select new Replay
                         {
                             ReplayId = r.ReplayId,
                             ErrorCode = r.ErrorCode,
                             Seed = r.Seed,
                             Version = r.Version,
                             KilledBy = r.KilledBy,
                         })
                         .ToList();

            var rowsAffected = 0;
            try
            {
                rowsAffected = await storeClient.SaveChangesAsync(model, true, cancellationToken);
            }
            // Violation of PRIMARY KEY constraint
            catch (SqlException ex) when (ex.Number == 2627)
            {
                return Conflict();
            }

            var content = new BulkStoreDTO { RowsAffected = rowsAffected };

            return Ok(content);
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                db.Dispose();
            }

            disposed = true;

            base.Dispose(disposing);
        }

        #endregion
    }
}
