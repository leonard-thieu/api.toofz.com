using System.Globalization;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ValueProviders;

namespace toofz.NecroDancer.Web.Api.Tests
{
    static class Util
    {
        public static ValueProviderResult CreateValueProviderResult(object rawValue)
        {
            return new ValueProviderResult(rawValue, rawValue.ToString(), CultureInfo.InvariantCulture);
        }

        public static ModelMetadata CreateModelMetadata<T>()
        {
            var provider = new EmptyModelMetadataProvider();

            return provider.GetMetadataForType(null, typeof(T));
        }
    }
}
