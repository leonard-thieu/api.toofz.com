using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    /// <summary>
    /// Methods for working with Crypt of the NecroDancer leaderboards.
    /// </summary>
    [RoutePrefix("leaderboards")]
    public sealed class LeaderboardsController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderboardsController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        public LeaderboardsController(ILeaderboardsContext db)
        {
            this.db = db;
        }

        readonly ILeaderboardsContext db;

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer leaderboards.
        /// </summary>
        /// <param name="products">
        /// Valid values are 'classic' and 'amplified'. 
        /// If not provided, returns leaderboards from all products.
        /// </param>
        /// <param name="modes">
        /// Valid values are 'standard', 'no-return', 'hard', 'phasing', 'randomzier', and 'mystery'. 
        /// If not provided, returns leaderboards from all modes.
        /// </param>
        /// <param name="runs">
        /// Valid values are 'score', 'speed', 'seeded-score', 'seeded-speed', and 'deathless'. 
        /// If not provided, returns leaderboards from all runs.
        /// </param>
        /// <param name="characters">
        /// Valid values are 'all-characters', 'all-characters-amplified', 'aria', 'bard', 'bolt', 'cadence', 'coda', 'diamond', 'dorian', 'dove', 'eli', 'mary', 'melody', 'monk', 'nocturna', 'story-mode', and 'tempo'. 
        /// If not provided, returns leaderboards from all characters.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer leaderboards.
        /// </returns>
        [ResponseType(typeof(LeaderboardsEnvelope))]
        [Route("")]
        public async Task<IHttpActionResult> GetLeaderboards(
            Products products,
            Modes modes,
            Runs runs,
            Characters characters,
            CancellationToken cancellationToken = default)
        {
            var query = from l in db.Leaderboards
                        where products.Contains(l.Product.Name)
                        where modes.Contains(l.Mode.Name)
                        where runs.Contains(l.Run.Name)
                        where characters.Contains(l.Character.Name)
                        orderby l.Product.Name, l.Character.Name, l.RunId
                        select new LeaderboardDTO
                        {
                            Id = l.LeaderboardId,
                            Product = l.Product.Name,
                            Character = l.Character.Name,
                            Mode = l.Mode.Name,
                            Run = l.Run.Name,
                            DisplayName = l.DisplayName,
                            UpdatedAt = l.LastUpdate,
                            Total = l.Entries.Count,
                        };

            var total = await query.CountAsync(cancellationToken);
            var leaderboards = await query.ToListAsync(cancellationToken);

            var content = new LeaderboardsEnvelope
            {
                Total = total,
                Leaderboards = leaderboards,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer leaderboard entries.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="lbid">The leaderboard ID.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a list of Crypt of the NecroDancer leaderboard entries.</returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// The leaderboard does not exist.
        /// </httpStatusCode>
        [ResponseType(typeof(LeaderboardEntriesDTO))]
        [Route("{lbid:int}/entries")]
        public async Task<IHttpActionResult> GetLeaderboardEntries(
            LeaderboardsPagination pagination,
            int lbid,
            CancellationToken cancellationToken = default)
        {
            var leaderboard = await (from l in db.Leaderboards
                                     where l.LeaderboardId == lbid
                                     select new LeaderboardDTO
                                     {
                                         Id = l.LeaderboardId,
                                         Product = l.Product.Name,
                                         Character = l.Character.Name,
                                         Mode = l.Mode.Name,
                                         Run = l.Run.Name,
                                         DisplayName = l.DisplayName,
                                         UpdatedAt = l.LastUpdate,
                                     })
                                     .FirstOrDefaultAsync(cancellationToken);
            if (leaderboard == null)
            {
                return NotFound();
            }

            var query = from e in db.Entries
                        where e.LeaderboardId == lbid
                        orderby e.Rank
                        select new
                        {
                            Player = new
                            {
                                e.Player.SteamId,
                                e.Player.Name,
                                e.Player.LastUpdate,
                                e.Player.Avatar,
                            },
                            Rank = e.Rank,
                            Score = e.Score,
                            End = new
                            {
                                e.Zone,
                                e.Level,
                            },
                            ReplayId = e.ReplayId,
                        };

            var total = await query.CountAsync(cancellationToken);
            var entriesPage = await query
                .Skip(pagination.Offset)
                .Take(pagination.Limit)
                .ToListAsync(cancellationToken);

            var replayIds = entriesPage.Select(entry => entry.ReplayId);
            var replays = await (from r in db.Replays
                                 where replayIds.Contains(r.ReplayId)
                                 select new
                                 {
                                     r.ReplayId,
                                     r.KilledBy,
                                     r.Version,
                                 }).ToListAsync(cancellationToken);

            var entries = (from e in entriesPage
                           join r in replays on e.ReplayId equals r.ReplayId into g
                           from x in g.DefaultIfEmpty()
                           select new EntryDTO
                           {
                               Player = new PlayerDTO
                               {
                                   Id = e.Player.SteamId.ToString(),
                                   DisplayName = e.Player.Name,
                                   UpdatedAt = e.Player.LastUpdate,
                                   Avatar = e.Player.Avatar,
                               },
                               Rank = e.Rank,
                               Score = e.Score,
                               End = new EndDTO
                               {
                                   Zone = e.End.Zone,
                                   Level = e.End.Level
                               },
                               KilledBy = x?.KilledBy,
                               Version = x?.Version,
                           }).ToList();

            var content = new LeaderboardEntriesDTO
            {
                Leaderboard = leaderboard,
                Total = total,
                Entries = entries,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer daily leaderboards.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="products">
        /// Valid values are 'classic' and 'amplified'. 
        /// If not provided, returns daily leaderboards from all products.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer daily leaderboards.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// A product is invalid.
        /// </httpStatusCode>
        [ResponseType(typeof(DailyLeaderboardsEnvelope))]
        [Route("dailies")]
        public async Task<IHttpActionResult> GetDailyLeaderboards(
            LeaderboardsPagination pagination,
            Products products,
            CancellationToken cancellationToken = default)
        {
            var query = from l in db.DailyLeaderboards
                        where products.Contains(l.Product.Name)
                        orderby l.Date descending, l.ProductId
                        select new DailyLeaderboardDTO
                        {
                            Id = l.LeaderboardId,
                            Date = l.Date,
                            UpdatedAt = l.LastUpdate,
                            IsProduction = l.IsProduction,
                            Product = l.Product.Name,
                        };

            var total = await query.CountAsync(cancellationToken);
            var leaderboards = await query
                .Skip(pagination.Offset)
                .Take(pagination.Limit)
                .ToListAsync(cancellationToken);

            var content = new DailyLeaderboardsEnvelope
            {
                Total = total,
                Leaderboards = leaderboards,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer daily leaderboard entries.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="lbid">The daily leaderboard ID.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a list of Crypt of the NecroDancer daily leaderboard entries.</returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// The daily leaderboard does not exist.
        /// </httpStatusCode>
        [ResponseType(typeof(DailyLeaderboardEntriesDTO))]
        [Route("dailies/{lbid:int}/entries")]
        public async Task<IHttpActionResult> GetDailyLeaderboardEntries(
            LeaderboardsPagination pagination,
            int lbid,
            CancellationToken cancellationToken = default)
        {
            var leaderboard = await (from l in db.DailyLeaderboards
                                     where l.LeaderboardId == lbid
                                     select new DailyLeaderboardDTO
                                     {
                                         Id = l.LeaderboardId,
                                         Date = l.Date,
                                         UpdatedAt = l.LastUpdate,
                                         Product = l.Product.Name,
                                         IsProduction = l.IsProduction,
                                     })
                                     .FirstOrDefaultAsync(cancellationToken);
            if (leaderboard == null)
            {
                return NotFound();
            }

            var query = from e in db.DailyEntries
                        where e.LeaderboardId == lbid
                        orderby e.Rank
                        select new
                        {
                            Player = new
                            {
                                e.Player.SteamId,
                                e.Player.Name,
                                e.Player.LastUpdate,
                                e.Player.Avatar,
                            },
                            Rank = e.Rank,
                            Score = e.Score,
                            End = new
                            {
                                e.Zone,
                                e.Level,
                            },
                            ReplayId = e.ReplayId,
                        };

            var total = await query.CountAsync(cancellationToken);
            var entriesPage = await query
                .Skip(pagination.Offset)
                .Take(pagination.Limit)
                .ToListAsync(cancellationToken);

            var replayIds = entriesPage.Select(entry => entry.ReplayId);
            var replays = await (from r in db.Replays
                                 where replayIds.Contains(r.ReplayId)
                                 select new
                                 {
                                     r.ReplayId,
                                     r.KilledBy,
                                     r.Version,
                                 })
                                 .ToListAsync(cancellationToken);

            var entries = (from e in entriesPage
                           join r in replays on e.ReplayId equals r.ReplayId into g
                           from x in g.DefaultIfEmpty()
                           select new EntryDTO
                           {
                               Player = new PlayerDTO
                               {
                                   Id = e.Player.SteamId.ToString(),
                                   DisplayName = e.Player.Name,
                                   UpdatedAt = e.Player.LastUpdate,
                                   Avatar = e.Player.Avatar,
                               },
                               Rank = e.Rank,
                               Score = e.Score,
                               End = new EndDTO
                               {
                                   Zone = e.End.Zone,
                                   Level = e.End.Level,
                               },
                               KilledBy = x?.KilledBy,
                               Version = x?.Version,
                           })
                           .ToList();

            var content = new DailyLeaderboardEntriesDTO
            {
                Leaderboard = leaderboard,
                Total = total,
                Entries = entries,
            };

            return Ok(content);
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
