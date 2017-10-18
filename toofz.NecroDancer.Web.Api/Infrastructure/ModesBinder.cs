using System;
using System.Collections.Generic;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class ModesBinder : CommaSeparatedValuesBinder<string>
    {
        public ModesBinder(IEnumerable<string> modes)
        {
            this.modes = modes ?? throw new ArgumentNullException(nameof(modes));
        }

        readonly IEnumerable<string> modes;

        protected override CommaSeparatedValues<string> GetModel() => new Modes(modes);
    }
}