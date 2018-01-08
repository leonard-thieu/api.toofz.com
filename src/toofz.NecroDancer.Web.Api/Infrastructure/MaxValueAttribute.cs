using System;
using System.ComponentModel.DataAnnotations;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    internal sealed class MaxValueAttribute : ValidationAttribute
    {
        public MaxValueAttribute(int max)
        {
            Max = max;
        }

        public int Max { get; }

        public override bool IsValid(object value)
        {
            var other = Convert.ToInt32(value);

            return other <= Max;
        }
    }
}