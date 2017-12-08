using System.Collections.Generic;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class RunsBinder : CommaSeparatedValuesBinder<string>
    {
        public RunsBinder(IEnumerable<string> runs)
        {
            this.runs = runs.ToList();
        }

        private readonly IEnumerable<string> runs;

        protected override CommaSeparatedValues<string> GetModel() => new Runs(runs);
    }
}