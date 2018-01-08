using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.EntityFrameworkCore;
using toofz.Data;
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
        public ReplaysController(ILeaderboardsContext db)
        {
            this.db = db;
        }

        private readonly ILeaderboardsContext db;

        // TODO: This is no longer needed for Replays Service. Determine if there's an public consumers of this and if not, 
        //       remove it.
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
            if (disposed) { return; }

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
