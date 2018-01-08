using System.Collections.Generic;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ProductsBinder : CommaSeparatedValuesBinder<string>
    {
        public ProductsBinder(IEnumerable<string> products)
        {
            this.products = products.ToList();
        }

        private readonly IEnumerable<string> products;

        protected override CommaSeparatedValues<string> GetModel() => new Products(products);
    }
}