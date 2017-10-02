using System;
using System.Collections.Generic;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class RunsBinder : CommaSeparatedValuesBinder
    {
        public RunsBinder(IEnumerable<string> runs)
        {
            this.runs = runs ?? throw new ArgumentNullException(nameof(runs));
        }

        readonly IEnumerable<string> runs;

        protected override CommaSeparatedValues GetModel() => new Runs(runs);
    }
}