using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    /// Methods for working with Crypt of the NecroDancer items.
    /// </summary>
    [RoutePrefix("items")]
    public sealed class ItemsController : ApiController
    {
        static readonly IEnumerable<string> RedChestSlots = new[] { "head", "hud", "hud_weapon", "action", "bomb", "shovel" };
        static readonly IEnumerable<string> PurpleChestSlots = new[] { "ring" };
        static readonly IEnumerable<string> BlackChestSlots = new[] { "feet" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="db">The NecroDancer context.</param>
        public ItemsController(INecroDancerContext db)
        {
            this.db = db;
        }

        readonly INecroDancerContext db;

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer items.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer items.
        /// </returns>
        [ResponseType(typeof(ItemsEnvelope))]
        [Route("")]
        public async Task<IHttpActionResult> GetItems(
            ItemsPagination pagination,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = from i in db.Items.AsNoTracking()
                            select i;

            var content = await GetItemsAsync(pagination, baseQuery, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer items in a specific category.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="category">The category of items to return.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer items in the category.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.BadRequest">
        /// Item category is invalid.
        /// </httpStatusCode>
        [ResponseType(typeof(ItemsEnvelope))]
        [Route("{category}")]
        public async Task<IHttpActionResult> GetItemsByCategory(
            ItemsPagination pagination,
            [ModelBinder(typeof(ItemCategoryBinder))] string category,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = from i in db.Items.AsNoTracking()
                            select i;
            switch (category)
            {
                case "armor": baseQuery = baseQuery.Where(i => i.IsArmor); break;
                case "consumable": baseQuery = baseQuery.Where(i => i.Consumable); break;
                case "feet": baseQuery = baseQuery.Where(i => i.Slot == "feet"); break;
                case "food": baseQuery = baseQuery.Where(i => i.IsFood); break;
                case "head": baseQuery = baseQuery.Where(i => i.Slot == "head"); break;
                case "rings": baseQuery = baseQuery.Where(i => i.Slot == "ring"); break;
                case "scrolls": baseQuery = baseQuery.Where(i => i.IsScroll); break;
                case "spells": baseQuery = baseQuery.Where(i => i.IsSpell); break;
                case "torches": baseQuery = baseQuery.Where(i => i.IsTorch); break;
                case "weapons": baseQuery = baseQuery.Where(i => i.IsWeapon); break;
            }

            var content = await GetItemsAsync(pagination, baseQuery, cancellationToken);

            return Ok(content);
        }

        /// <summary>
        /// Gets a list of Crypt of the NecroDancer items in a specific subcategory.
        /// </summary>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="filter">The subcategory to get items for.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// Returns a list of Crypt of the NecroDancer items in the subcategory.
        /// </returns>
        /// <httpStatusCode cref="HttpStatusCode.BadRequest">
        /// Item category is invalid.
        /// </httpStatusCode>
        /// <httpStatusCode cref="HttpStatusCode.BadRequest">
        /// Item subcategory is invalid.
        /// </httpStatusCode>
        [ResponseType(typeof(ItemsEnvelope))]
        [Route("{category}/{subcategory}")]
        public async Task<IHttpActionResult> GetItemsBySubcategory(
            ItemsPagination pagination,
            ItemSubcategoryFilter filter,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = from i in db.Items.AsNoTracking()
                            select i;
            switch (filter.Category)
            {
                case "weapons":
                    switch (filter.Subcategory)
                    {
                        case "bows": baseQuery = baseQuery.Where(w => w.IsBow); break;
                        case "broadswords": baseQuery = baseQuery.Where(w => w.IsBroadsword); break;
                        case "cats": baseQuery = baseQuery.Where(w => w.IsCat); break;
                        case "crossbows": baseQuery = baseQuery.Where(w => w.IsCrossbow); break;
                        case "daggers": baseQuery = baseQuery.Where(w => w.IsDagger); break;
                        case "flails": baseQuery = baseQuery.Where(w => w.IsFlail); break;
                        case "longswords": baseQuery = baseQuery.Where(w => w.IsLongsword); break;
                        case "rapiers": baseQuery = baseQuery.Where(w => w.IsRapier); break;
                        case "spears": baseQuery = baseQuery.Where(w => w.IsSpear); break;
                        case "whips": baseQuery = baseQuery.Where(w => w.IsWhip); break;
                    }
                    break;
                case "chest":
                    switch (filter.Subcategory)
                    {
                        case "red": baseQuery = baseQuery.Where(i => (i.IsFood || i.IsTorch || i.IsShovel || RedChestSlots.Contains(i.Slot)) && !i.IsScroll); break;
                        case "purple": baseQuery = baseQuery.Where(i => i.IsSpell || i.IsScroll || PurpleChestSlots.Contains(i.Slot)); break;
                        case "black": baseQuery = baseQuery.Where(i => i.IsArmor || i.IsWeapon || BlackChestSlots.Contains(i.Slot)); break;
                        case "mimic": baseQuery = baseQuery.Where(i => true); break;
                    }
                    break;
            }

            var content = await GetItemsAsync(pagination, baseQuery, cancellationToken);

            return Ok(content);
        }

        async Task<ItemsEnvelope> GetItemsAsync(
            ItemsPagination pagination,
            IQueryable<Item> baseQuery,
            CancellationToken cancellationToken)
        {
            var query = from i in baseQuery
                        orderby i.Name
                        select new ItemDTO
                        {
                            Name = i.Name,
                            DisplayName = i.DisplayName,
                            Slot = i.Slot,
                            Cost = i.CoinCost,
                            Unlock = i.DiamondCost,
                        };

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Page(pagination)
                .ToListAsync(cancellationToken);

            return new ItemsEnvelope
            {
                Total = total,
                Items = items,
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
