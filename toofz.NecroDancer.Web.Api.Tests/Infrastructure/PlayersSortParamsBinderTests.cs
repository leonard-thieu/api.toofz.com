using System.Web.Http.Controllers;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class PlayersSortParamsBinderTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var binder = new PlayersSortParamsBinder();

                // Assert
                Assert.IsInstanceOfType(binder, typeof(PlayersSortParamsBinder));
            }
        }

        [TestClass]
        public class GetModelMethod
        {
            [TestMethod]
            public void ReturnsModel()
            {
                // Arrange
                var binder = new PlayersSortParamsBinder();
                HttpActionContext actionContext = null;
                var modelName = "myModelName";
                var mockValueProvider = new Mock<IValueProvider>();
                mockValueProvider.Setup(v => v.GetValue("myModelName")).Returns(Util.CreateValueProviderResult("id"));
                var valueProvider = mockValueProvider.Object;
                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(PlayersSortParams));
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
                Assert.IsInstanceOfType(model, typeof(PlayersSortParams));
            }
        }
    }
}
