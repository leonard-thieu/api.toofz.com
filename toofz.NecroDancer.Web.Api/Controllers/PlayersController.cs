using System.Collections.Generic;
using System.Data.Entity;
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
    /// Methods for working with Steam players.
    /// </summary>
    [RoutePrefix("players")]
    public sealed class PlayersController : ApiController
    {
        private static Dictionary<string, string> SortKeySelectorMap = new Dictionary<string, string>
        {
            ["id"] = $"{nameof(Player.SteamId)}",
            ["display_name"] = $"{nameof(Player.Name)}",
            ["updated_at"] = $"{nameof(Player.LastUpdate)}",
            ["entries"] = $"{nameof(Player.Entries)}.{nameof(List<Entry>.Count)}",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersController"/> class.
        /// </summary>
        /// <param name="db">The leaderboards context.</param>
        /// <param name="storeClient">The leaderboards store client.</param>
        public PlayersController(
            ILeaderboardsContext db,
            ILeaderboardsStoreClient storeClient)
        {
            this.db = db;
            this.storeClient = storeClient;
        }

        private readonly ILeaderboardsContext db;
        private readonly ILeaderboardsStoreClient storeClient;

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
            PlayersSortParams sort,
            string q = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Player> query = db.Players.AsNoTracking();
            // Filtering
            if (q != null) { query = query.Where(p => p.Name.StartsWith(q)); }
            // Sorting
            query = query.OrderBy(SortKeySelectorMap, sort);

            var total = await query.CountAsync(cancellationToken);
            var players = await (from p in query
                                 select new PlayerDTO
                                 {
                                     Id = p.SteamId.ToString(),
                                     UpdatedAt = p.LastUpdate,
                                     DisplayName = p.Name,
                                     Avatar = p.Avatar,
                                 })
                                 .Page(pagination)
                                 .ToListAsync(cancellationToken);

            var content = new PlayersEnvelope
            {
                Total = total,
                Players = players,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a Steam player.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a Steam player.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(PlayerEnvelope))]
        [Route("{steamId}")]
        public async Task<IHttpActionResult> GetPlayer(
            long steamId,
            CancellationToken cancellationToken = default)
        {
            // Validate steamId
            var player = await (from p in db.Players.AsNoTracking()
                                where p.SteamId == steamId
                                select new PlayerDTO
                                {
                                    Id = p.SteamId.ToString(),
                                    UpdatedAt = p.LastUpdate,
                                    DisplayName = p.Name,
                                    Avatar = p.Avatar,
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            if (player == null)
            {
                return NotFound();
            }

            var content = new PlayerEnvelope
            {
                Player = player,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a Steam player's leaderboard entries.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="lbids">
        /// Comma-separated leaderboard IDs.
        /// </param>
        /// <param name="products">
        /// Valid values are 'classic' and 'amplified'. 
        /// If not provided, returns leaderboards from all products.
        /// </param>
        /// <param name="production">
        /// If true, returns production leaderboards. If false, returns Early Access leaderboards.
        /// If not provided, returns both production and Early Access leaderboards.
        /// </param>
        /// <param name="coOp">
        /// If true, returns Co-op leaderboards. If false, returns non-Co-op leaderboards.
        /// If not provided, returns both Co-op and non-Co-op leaderboards.
        /// </param>
        /// <param name="customMusic">
        /// If true, returns Custom Music leaderboards. If false, returns non-Custom Music leaderboards.
        /// If not provided, returns both Custom Music and non-Custom Music leaderboards.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a Steam player's leaderboard entries.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// One or more leaderboards were not found.
        /// </httpStatusCode>
        [ResponseType(typeof(PlayerEntriesDTO))]
        [Route("{steamId}/entries")]
        public async Task<IHttpActionResult> GetPlayerEntries(
            long steamId,
            LeaderboardIdParams lbids,
            Products products,
            bool? production = null,
            bool? coOp = null,
            bool? customMusic = null,
            CancellationToken cancellationToken = default)
        {
            // Validate steamId
            var player = await (from p in db.Players.AsNoTracking()
                                where p.SteamId == steamId
                                select new PlayerDTO
                                {
                                    Id = p.SteamId.ToString(),
                                    UpdatedAt = p.LastUpdate,
                                    DisplayName = p.Name,
                                    Avatar = p.Avatar,
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            if (player == null)
            {
                return NotFound();
            }

            // Validate lbids
            var validLbids = await (from l in db.Leaderboards.AsNoTracking()
                                    where lbids.Contains(l.LeaderboardId)
                                    select l.LeaderboardId)
                                    .ToListAsync(cancellationToken);
            var invalidLbids = lbids.Except(validLbids).ToList();
            if (invalidLbids.Any())
            {
                return NotFound();
            }

            var query = from e in db.Entries.AsNoTracking()
                        where e.SteamId == steamId
                        where products.Contains(e.Leaderboard.Product.Name)
                        let l = e.Leaderboard
                        orderby l.Product.ProductId descending, l.Mode.ModeId, l.Run.RunId, l.Character.Name
                        select e;
            if (lbids.Any()) { query = query.Where(e => lbids.Contains(e.Leaderboard.LeaderboardId)); }
            if (production != null) { query = query.Where(e => e.Leaderboard.IsProduction == production); }
            if (coOp != null) { query = query.Where(e => e.Leaderboard.IsCoOp == coOp); }
            if (customMusic != null) { query = query.Where(e => e.Leaderboard.IsCustomMusic == customMusic); }

            var total = await query.CountAsync(cancellationToken);
            var entries = await (from e in query
                                 let l = e.Leaderboard
                                 let r = e.Replay
                                 select new EntryDTO
                                 {
                                     Leaderboard = new LeaderboardDTO
                                     {
                                         Id = l.LeaderboardId,
                                         UpdatedAt = l.LastUpdate,
                                         Name = l.Name,
                                         DisplayName = l.DisplayName,
                                         IsProduction = l.IsProduction,
                                         ProductName = l.Product.Name,
                                         Product = new ProductDTO
                                         {
                                             Id = l.Product.ProductId,
                                             Name = l.Product.Name,
                                             DisplayName = l.Product.DisplayName,
                                         },
                                         ModeName = l.Mode.Name,
                                         Mode = new ModeDTO
                                         {
                                             Id = l.Mode.ModeId,
                                             Name = l.Mode.Name,
                                             DisplayName = l.Mode.DisplayName,
                                         },
                                         RunName = l.Run.Name,
                                         Run = new RunDTO
                                         {
                                             Id = l.Run.RunId,
                                             Name = l.Run.Name,
                                             DisplayName = l.Run.DisplayName,
                                         },
                                         CharacterName = l.Character.Name,
                                         Character = new CharacterDTO
                                         {
                                             Id = l.Character.CharacterId,
                                             Name = l.Character.Name,
                                             DisplayName = l.Character.DisplayName,
                                         },
                                         IsCoOp = l.IsCoOp,
                                         IsCustomMusic = l.IsCustomMusic,
                                         Total = l.Entries.Count(),
                                     },
                                     Rank = e.Rank,
                                     Score = e.Score,
                                     End = new EndDTO
                                     {
                                         Zone = e.Zone,
                                         Level = e.Level,
                                     },
                                     KilledBy = r.KilledBy,
                                     Version = r.Version,
                                 })
                                 .ToListAsync(cancellationToken);

            var content = new PlayerEntriesDTO
            {
                Player = player,
                Total = total,
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
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// An entry for the player on the leaderboard was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(EntryDTO))]
        [Route("{steamId}/entries/{lbid}")]
        public async Task<IHttpActionResult> GetPlayerEntry(
            int lbid,
            long steamId,
            CancellationToken cancellationToken = default)
        {
            var entry = await (from e in db.Entries.AsNoTracking()
                               where e.LeaderboardId == lbid
                               where e.SteamId == steamId
                               let l = e.Leaderboard
                               let p = e.Player
                               let r = e.Replay
                               select new EntryDTO
                               {
                                   Leaderboard = new LeaderboardDTO
                                   {
                                       Id = l.LeaderboardId,
                                       UpdatedAt = l.LastUpdate,
                                       Name = l.Name,
                                       DisplayName = l.DisplayName,
                                       IsProduction = l.IsProduction,
                                       ProductName = l.Product.Name,
                                       Product = new ProductDTO
                                       {
                                           Id = l.Product.ProductId,
                                           Name = l.Product.Name,
                                           DisplayName = l.Product.DisplayName,
                                       },
                                       ModeName = l.Mode.Name,
                                       Mode = new ModeDTO
                                       {
                                           Id = l.Mode.ModeId,
                                           Name = l.Mode.Name,
                                           DisplayName = l.Mode.DisplayName,
                                       },
                                       RunName = l.Run.Name,
                                       Run = new RunDTO
                                       {
                                           Id = l.Run.RunId,
                                           Name = l.Run.Name,
                                           DisplayName = l.Run.DisplayName,
                                       },
                                       CharacterName = l.Character.Name,
                                       Character = new CharacterDTO
                                       {
                                           Id = l.Character.CharacterId,
                                           Name = l.Character.Name,
                                           DisplayName = l.Character.DisplayName,
                                       },
                                       IsCoOp = l.IsCoOp,
                                       IsCustomMusic = l.IsCustomMusic,
                                       Total = l.Entries.Count(),
                                   },
                                   Player = new PlayerDTO
                                   {
                                       Id = p.SteamId.ToString(),
                                       UpdatedAt = p.LastUpdate,
                                       DisplayName = p.Name,
                                       Avatar = p.Avatar,
                                   },
                                   Rank = e.Rank,
                                   Score = e.Score,
                                   End = new EndDTO
                                   {
                                       Zone = e.Zone,
                                       Level = e.Level,
                                   },
                                   KilledBy = r.KilledBy,
                                   Version = r.Version,
                               })
                               .FirstOrDefaultAsync(cancellationToken);
            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry);
        }

        /// <summary>
        /// Gets a Steam player's daily leaderboard entries.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="lbids">
        /// Comma-separated leaderboard IDs.
        /// </param>
        /// <param name="products">
        /// Valid values are 'classic' and 'amplified'. 
        /// If not provided, returns daily leaderboards from all products.
        /// </param>
        /// <param name="production">
        /// If true, returns production leaderboards. If false, returns Early Access leaderboards.
        /// If not provided, returns both production and Early Access leaderboards.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a Steam player's daily leaderboard entries.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// One or more leaderboards were not found.
        /// </httpStatusCode>
        [ResponseType(typeof(PlayerDailyEntriesDTO))]
        [Route("{steamId}/entries/dailies")]
        public async Task<IHttpActionResult> GetPlayerDailyEntries(
            long steamId,
            LeaderboardIdParams lbids,
            Products products,
            bool? production = null,
            CancellationToken cancellationToken = default)
        {
            // Validate steamId
            var player = await (from p in db.Players.AsNoTracking()
                                where p.SteamId == steamId
                                select new PlayerDTO
                                {
                                    Id = p.SteamId.ToString(),
                                    UpdatedAt = p.LastUpdate,
                                    DisplayName = p.Name,
                                    Avatar = p.Avatar,
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            if (player == null)
            {
                return NotFound();
            }

            // Validate lbids
            var validLbids = await (from l in db.DailyLeaderboards.AsNoTracking()
                                    where lbids.Contains(l.LeaderboardId)
                                    select l.LeaderboardId)
                                    .ToListAsync(cancellationToken);
            var invalidLbids = lbids.Except(validLbids).ToList();
            if (invalidLbids.Any())
            {
                return NotFound();
            }

            var query = from e in db.DailyEntries.AsNoTracking()
                        where e.SteamId == steamId
                        where products.Contains(e.Leaderboard.Product.Name)
                        select e;
            if (lbids.Any()) { query = query.Where(e => lbids.Contains(e.Leaderboard.LeaderboardId)); }
            if (production != null) { query = query.Where(e => e.Leaderboard.IsProduction == production); }

            var total = await query.CountAsync(cancellationToken);
            var entries = await (from e in query
                                 let l = e.Leaderboard
                                 let r = e.Replay
                                 select new DailyEntryDTO
                                 {
                                     Leaderboard = new DailyLeaderboardDTO
                                     {
                                         Id = l.LeaderboardId,
                                         UpdatedAt = l.LastUpdate,
                                         Name = l.Name,
                                         DisplayName = l.DisplayName,
                                         IsProduction = l.IsProduction,
                                         ProductName = l.Product.Name,
                                         Product = new ProductDTO
                                         {
                                             Id = l.Product.ProductId,
                                             Name = l.Product.Name,
                                             DisplayName = l.Product.DisplayName,
                                         },
                                         Date = l.Date,
                                         Total = l.Entries.Count(),
                                     },
                                     Rank = e.Rank,
                                     Score = e.Score,
                                     End = new EndDTO
                                     {
                                         Zone = e.Zone,
                                         Level = e.Level,
                                     },
                                     KilledBy = r.KilledBy,
                                     Version = r.Version,
                                 })
                                 .ToListAsync(cancellationToken);

            var content = new PlayerDailyEntriesDTO
            {
                Player = player,
                Total = total,
                Entries = entries,
            };

            return Ok(content);
        }

        /// <summary>
        /// Gets a player's leaderboard entry for a daily leaderboard.
        /// </summary>
        /// <param name="lbid">The ID of the leaderboard.</param>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a player's leaderboard entry for a daily leaderboard.</returns>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// A player with that Steam ID was not found.
        /// </httpStatusCode>
        /// <httpStatusCode cref="HttpStatusCode.NotFound">
        /// An entry for the player on the dao;y leaderboard was not found.
        /// </httpStatusCode>
        [ResponseType(typeof(EntryDTO))]
        [Route("{steamId}/entries/dailies/{lbid}")]
        public async Task<IHttpActionResult> GetPlayerDailyEntry(
            int lbid,
            long steamId,
            CancellationToken cancellationToken = default)
        {
            var entry = await (from e in db.DailyEntries.AsNoTracking().Include(x => x.Player)
                               where e.LeaderboardId == lbid
                               where e.SteamId == steamId
                               let l = e.Leaderboard
                               let p = e.Player
                               let r = e.Replay
                               select new DailyEntryDTO
                               {
                                   Leaderboard = new DailyLeaderboardDTO
                                   {
                                       Id = l.LeaderboardId,
                                       UpdatedAt = l.LastUpdate,
                                       Name = l.Name,
                                       DisplayName = l.DisplayName,
                                       IsProduction = l.IsProduction,
                                       ProductName = l.Product.Name,
                                       Product = new ProductDTO
                                       {
                                           Id = l.Product.ProductId,
                                           Name = l.Product.Name,
                                           DisplayName = l.Product.DisplayName,
                                       },
                                       Date = l.Date,
                                       Total = l.Entries.Count(),
                                   },
                                   Player = new PlayerDTO
                                   {
                                       Id = p.SteamId.ToString(),
                                       UpdatedAt = p.LastUpdate,
                                       DisplayName = p.Name,
                                       Avatar = p.Avatar,
                                   },
                                   Rank = e.Rank,
                                   Score = e.Score,
                                   End = new EndDTO
                                   {
                                       Zone = e.Zone,
                                       Level = e.Level,
                                   },
                                   KilledBy = r.KilledBy,
                                   Version = r.Version,
                               })
                               .FirstOrDefaultAsync(cancellationToken);
            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry);
        }

        /// <summary>
        /// Updates Steam players.
        /// </summary>
        /// <param name="players">A list of players.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns the number of Steam players updated.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.BadRequest">
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
                         })
                         .ToList();
            var rowsAffected = await storeClient.BulkUpsertAsync(model, cancellationToken);

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
