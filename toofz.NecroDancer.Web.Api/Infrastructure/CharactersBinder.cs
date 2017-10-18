using System;
using System.Collections.Generic;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    public sealed class CharactersBinder : CommaSeparatedValuesBinder<string>
    {
        public CharactersBinder(IEnumerable<string> characters)
        {
            this.characters = characters ?? throw new ArgumentNullException(nameof(characters));
        }

        readonly IEnumerable<string> characters;

        protected override CommaSeparatedValues<string> GetModel() => new Characters(characters);
    }
}