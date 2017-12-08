using System;
using System.Web.Http.ValueProviders;

namespace toofz.NecroDancer.Web.Api
{
    /// <summary>
    /// Contains extension methods for <see cref="ValueProviderResult"/>.
    /// </summary>
    internal static class ValueProviderResultExtensions
    {
        /// <summary>
        /// Converts the value that is encapsulated by this result to the specified type.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="result">
        /// The <see cref="ValueProviderResult"/> to convert.
        /// </param>
        /// <returns>The converted value.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="result"/> is null.
        /// </exception>
        public static T ConvertTo<T>(this ValueProviderResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            try
            {
                return (T)result.ConvertTo(typeof(T));
            }
            catch
            {
                return default;
            }
        }
    }
}