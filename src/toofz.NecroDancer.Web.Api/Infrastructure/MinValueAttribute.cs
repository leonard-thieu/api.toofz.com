using System;
using System.ComponentModel.DataAnnotations;

namespace toofz.NecroDancer.Web.Api.Infrastructure
{
    internal sealed class MinValueAttribute : ValidationAttribute
    {
        public MinValueAttribute(int min)
        {
            Min = min;
        }

        public int Min { get; }

        public override bool IsValid(object value)
        {
            var other = Convert.ToInt32(value);

            return other >= Min;
        }
    }
}