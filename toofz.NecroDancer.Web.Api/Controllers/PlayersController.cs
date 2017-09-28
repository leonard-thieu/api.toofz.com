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
    /// Methods for working with Steam players.
    /// </summary>
    [RoutePrefix("players")]
    public sealed class PlayersController : ApiController
    {
        static IReadOnlyDictionary<string, string> SortKeySelectorMap = new Dictionary<string, string>
        {
            { "id", $"{nameof(Player.SteamId)}" },
            { "display_name", $"{nameof(Player.Name)}" },
            { "updated_at", $"{nameof(Player.LastUpdate)}" },
            { "entries", $"{nameof(Player.Entries)}.{nameof(List<Entry>.Count)}" },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        /// <param name="storeClient">The leaderboards store client.</param>
        /// <param name="leaderboardHeaders">Leaderboard headers.</param>
        public PlayersController(
            ILeaderboardsContext db,
            ILeaderboardsStoreClient storeClient,
            LeaderboardHeaders leaderboardHeaders)
        {
            this.db = db;
            this.storeClient = storeClient;
            this.leaderboardHeaders = leaderboardHeaders;
        }

        readonly ILeaderboardsContext db;
        readonly ILeaderboardsStoreClient storeClient;
        readonly LeaderboardHeaders leaderboardHeaders;

        /// <summary>
        /// Search for Steam players.
        /// </summary>
        /// <param name="q">A search query.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="sort">
        /// Comma-separated values of properties to sort by. Properties may be prefixed with "-" to sort descending.
        /// Valid properties are "id", "display_name", "updated_at", and "entries".
        /// If not provided, results will sorted using "-entries,display_name,id".
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Steam players that match the search query.
        /// </returns>
        [ResponseType(typeof(PlayersEnvelope))]
        [Route("")]
        public async Task<IHttpActionResult> GetPlayers(
            PlayersPagination pagination,
            string q = null,
            string sort = "-entries,display_name,id",
            CancellationToken cancellationToken = default)
        {
            IQueryable<Player> queryBase = db.Players;
            // Filtering
            if (q != null)
            {
                queryBase = queryBase.Where(p => p.Name.StartsWith(q));
            }
            // Sorting
            if (queryBase.TryApplySort(sort, SortKeySelectorMap, out var sorted))
            {
                queryBase = sorted;
            }

            var query = from p in queryBase
                        select new PlayerDTO
                        {
                            Id = p.SteamId.ToString(),
                            DisplayName = p.Name,
                            UpdatedAt = p.LastUpdate,
                            Avatar = p.Avatar,
                        };

            var total = await query.CountAsync(cancellationToken);
            var players = await query
                .Skip(pagination.offset)
                .Take(pagination.limit)
                .ToListAsync(cancellationToken);

            var content = new PlayersEnvelope
            {
                Total = total,
                Players = players,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a Steam player's leaderboard entries.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a Steam player's leaderboard entries.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(PlayerEntriesDTO))]
        [Route("{steamId}/entries")]
        public async Task<IHttpActionResult> GetPlayer(
            long steamId,
            CancellationToken cancellationToken = default)
        {
            var player = db.Players.FirstOrDefault(p => p.SteamId == steamId);
            if (player == null)
            {
                return NotFound();
            }

            var playerEntries = await (from e in db.Entries
                                       where e.SteamId == steamId
                                       select new
                                       {
                                           Leaderboard = new
                                           {
                                               e.Leaderboard.LeaderboardId,
                                               e.Leaderboard.CharacterId,
                                               e.Leaderboard.RunId,
                                               e.Leaderboard.LastUpdate,
                                           },
                                           Rank = e.Rank,
                                           Score = e.Score,
                                           End = new
                                           {
                                               e.Zone,
                                               e.Level,
                                           },
                                           ReplayId = e.ReplayId,
                                       }).ToListAsync(cancellationToken);

            var replayIds = playerEntries.Select(entry => entry.ReplayId);
            var replays = await (from r in db.Replays
                                 where replayIds.Contains(r.ReplayId)
                                 select new
                                 {
                                     r.ReplayId,
                                     r.KilledBy,
                                     r.Version,
                                 }).ToListAsync(cancellationToken);

            var entries = (from e in playerEntries
                           join r in replays on e.ReplayId equals r.ReplayId into g
                           from x in g.DefaultIfEmpty()
                           join h in leaderboardHeaders on e.Leaderboard.LeaderboardId equals h.Id
                           orderby h.Product, e.Leaderboard.RunId, h.Character
                           select new EntryDTO
                           {
                               Leaderboard = new LeaderboardDTO
                               {
                                   Id = h.Id,
                                   Product = h.Product,
                                   Character = h.Character,
                                   Mode = h.Mode,
                                   Run = h.Run,
                                   UpdatedAt = e.Leaderboard.LastUpdate,
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
                           }).ToList();

            var content = new PlayerEntriesDTO
            {
                Player = new PlayerDTO
                {
                    Id = player.SteamId.ToString(),
                    DisplayName = player.Name,
                    UpdatedAt = player.LastUpdate,
                    Avatar = player.Avatar,
                },
                Total = entries.Count,
                Entries = entries,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a player's leaderboard entry for a leaderboard.
        /// </summary>
        /// <param name="lbid">The ID of the leaderboard.</param>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a player's leaderboard entry for a leaderboard.</returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.NotFound">
        /// An entry for the player on the leaderboard was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(EntryDTO))]
        [Route("{steamId}/entries/{lbid:int}")]
        public async Task<IHttpActionResult> GetPlayerLeaderboardEntry(
            int lbid,
            long steamId,
            CancellationToken cancellationToken = default)
        {
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

            var playerEntry = await query.FirstOrDefaultAsync(e => e.Player.SteamId == steamId, cancellationToken);
            if (playerEntry == null)
            {
                return NotFound();
            }

            var replay = await (from r in db.Replays
                                where r.ReplayId == playerEntry.ReplayId
                                select new
                                {
                                    r.KilledBy,
                                    r.Version,
                                })
                                .FirstOrDefaultAsync(cancellationToken);

            var content = new EntryDTO
            {
                Player = new PlayerDTO
                {
                    Id = playerEntry.Player.SteamId.ToString(),
                    DisplayName = playerEntry.Player.Name,
                    UpdatedAt = playerEntry.Player.LastUpdate,
                    Avatar = playerEntry.Player.Avatar,
                },
                Rank = playerEntry.Rank,
                Score = playerEntry.Score,
                End = new EndDTO
                {
                    Zone = playerEntry.End.Zone,
                    Level = playerEntry.End.Level,
                },
                KilledBy = replay?.KilledBy,
                Version = replay?.Version,
            };

            return Ok(content);
        }

        /// <summary>
        /// Updates Steam players.
        /// </summary>
        /// <param name="players">A list of players.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns the number of Steam players updated.
        /// </returns>
        /// <httpStatusCode cref="System.Net.HttpStatusCode.BadRequest">
        /// Any players failed validation.
        /// </httpStatusCode>
        [ResponseType(typeof(BulkStoreDTO))]
        [Route("")]
        [Authorize(Users = "PlayersService")]
        public async Task<IHttpActionResult> PostPlayers(
            IEnumerable<PlayerModel> players,
            CancellationToken cancellationToken = default)
        {
            var model = (from p in players
                         select new Player
                         {
                             SteamId = p.SteamId.Value,
                             Exists = p.Exists,
                             Name = p.Name,
                             LastUpdate = p.LastUpdate,
                             Avatar = p.Avatar,
                         }).ToList();
            var rowsAffected = await storeClient.SaveChangesAsync(model, true, cancellationToken);

            var content = new BulkStoreDTO { RowsAffected = rowsAffected };

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
