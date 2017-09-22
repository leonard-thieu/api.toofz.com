using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    /// <summary>
    /// Methods for working with Crypt of the NecroDancer enemies.
    /// </summary>
    [RoutePrefix("enemies")]
    public sealed class EnemiesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemiesController"/> class.
        /// </summary>
        /// <param name="db">The NecroDancer context.</param>
        public EnemiesController(NecroDancerContext db)
        {
            this.db = db;
        }

        readonly NecroDancerContext db;

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer enemies.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer enemies.
        /// </returns>
        [ResponseType(typeof(EnemiesDTO))]
        [Route("")]
        public async Task<IHttpActionResult> GetEnemies(
            EnemiesPagination pagination,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = from e in db.Enemies
                            select e;
            var content = await GetEnemiesAsync(pagination, baseQuery, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer enemies with the specified attribute.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="attribute">
        /// The enemy's attribute.
        /// Valid values are 'boss', 'bounce-on-movement-fail', 'floating', 'ignore-liquids', 'ignore-walls', 'is-monkey-like', 'massive', and 'miniboss'.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer enemies with the attribute.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// Enemy attribute is invalid.
        /// </httpStatusCode>
        [ResponseType(typeof(EnemiesDTO))]
        [Route("{attribute}")]
        public async Task<IHttpActionResult> GetEnemies(
            EnemiesPagination pagination,
            [ModelBinder(typeof(EnemyAttributeBinder))] string attribute,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = from e in db.Enemies
                            select e;
            switch (attribute)
            {
                case "boss": baseQuery = baseQuery.Where(e => e.OptionalStats.Boss); break;
                case "bounce-on-movement-fail": baseQuery = baseQuery.Where(e => e.OptionalStats.BounceOnMovementFail); break;
                case "floating": baseQuery = baseQuery.Where(e => e.OptionalStats.Floating); break;
                case "ignore-liquids": baseQuery = baseQuery.Where(e => e.OptionalStats.IgnoreLiquids); break;
                case "ignore-walls": baseQuery = baseQuery.Where(e => e.OptionalStats.IgnoreWalls); break;
                case "is-monkey-like": baseQuery = baseQuery.Where(e => e.OptionalStats.IsMonkeyLike); break;
                case "massive": baseQuery = baseQuery.Where(e => e.OptionalStats.Massive); break;
                case "miniboss": baseQuery = baseQuery.Where(e => e.OptionalStats.Miniboss); break;
            }
            var content = await GetEnemiesAsync(pagination, baseQuery, cancellationToken);

            return Ok(content);
        }

        async Task<EnemiesDTO> GetEnemiesAsync(
            EnemiesPagination pagination,
            IQueryable<Enemy> baseQuery,
            CancellationToken cancellationToken)
        {
            var query = from e in baseQuery
                        orderby e.ElementName, e.Type
                        select new EnemyDTO
                        {
                            Name = e.ElementName,
                            Type = e.Type,
                            DisplayName = e.Name,
                            Health = e.Stats.Health,
                            Damage = e.Stats.DamagePerHit,
                            BeatsPerMove = e.Stats.BeatsPerMove,
                            Drops = e.Stats.CoinsToDrop,
                        };

            var total = await query.CountAsync(cancellationToken);

            var enemies = await query
                .Skip(pagination.offset)
                .Take(pagination.limit)
                .ToListAsync(cancellationToken);

            return new EnemiesDTO
            {
                Total = total,
                Enemies = enemies,
            };
        }

        #region IDisposable Members

        bool disposed;

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
