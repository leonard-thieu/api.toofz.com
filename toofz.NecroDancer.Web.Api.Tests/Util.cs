using System.Collections.Generic;
using System.Web.Http.Metadata;
using System.Web.Http.Metadata.Providers;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Web.Api.Tests
{
    internal static class Util
    {
        public static ModelMetadata CreateModelMetadata<T>()
        {
            var provider = new EmptyModelMetadataProvider();

            return provider.GetMetadataForType(null, typeof(T));
        }

        public static IEnumerable<T> ReadJsonArray<T>(string json)
            where T : class
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }
    }
}
