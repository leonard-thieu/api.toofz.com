using System.Collections.Generic;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ModesBinder : CommaSeparatedValuesBinder<string>
    {
        public ModesBinder(IEnumerable<string> modes)
        {
            this.modes = modes.ToList();
        }

        private readonly IEnumerable<string> modes;

        protected override CommaSeparatedValues<string> GetModel() => new Modes(modes);
    }
}