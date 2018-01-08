using System.Web.Http.Controllers;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class LeaderboardIdParamsBinderTests
    {
        public class Constructor
        {
            [DisplayFact(nameof(LeaderboardIdParamsBinder))]
            public void ReturnsLeaderboardIdParamsBinder()
            {
                // Arrange -> Act
                var binder = new LeaderboardIdParamsBinder();

                // Assert
                Assert.IsAssignableFrom<LeaderboardIdParamsBinder>(binder);
            }
        }

        public class GetModelMethod
        {
            [DisplayFact(nameof(LeaderboardIdParams))]
            public void ReturnsLeaderboardIdParams()
            {
                // Arrange
                var binder = new LeaderboardIdParamsBinder();
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("2047616,2047493");
                var valueProvider = mockValueProvider.Object;
                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(LeaderboardIdParams));
                var bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = valueProvider,
                    ModelMetadata = modelMetadata,
                };

                // Act
                binder.BindModel(actionContext, bindingContext);

                // Assert
                var model = bindingContext.Model;
                Assert.IsAssignableFrom<LeaderboardIdParams>(model);
            }
        }
    }
}
