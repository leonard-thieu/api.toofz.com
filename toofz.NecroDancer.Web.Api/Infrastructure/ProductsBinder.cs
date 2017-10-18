using System;
using System.Collections.Generic;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ProductsBinder : CommaSeparatedValuesBinder<string>
    {
        public ProductsBinder(IEnumerable<string> products)
        {
            this.products = products ?? throw new ArgumentNullException(nameof(products));
        }

        readonly IEnumerable<string> products;

        protected override CommaSeparatedValues<string> GetModel() => new Products(products);
    }
}