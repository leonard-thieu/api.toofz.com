using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;

namespace toofz.NecroDancer.Web.Api.Tests
{
    internal static class Util
    {
        public static ModelMetadata CreateModelMetadata<T>()
        {
            var provider = new EmptyModelMetadataProvider();

            return provider.GetMetadataForType(null, typeof(T));
        }
    }
}
