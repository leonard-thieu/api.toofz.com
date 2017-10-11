using System.Globalization;
using System.Web.Http.ValueProviders;
using Moq.Language;
using Moq.Language.Flow;

namespace toofz.NecroDancer.Web.Api.Tests
{
    static class IReturnsExtensions
    {
        public static IReturnsResult<TMock> ReturnsValueProviderResult<TMock>(this IReturns<TMock, ValueProviderResult> returns, object rawValue)
            where TMock : class
        {
            var result = new ValueProviderResult(rawValue, rawValue.ToString(), CultureInfo.InvariantCulture);

            return returns.Returns(result);
        }
    }
}
