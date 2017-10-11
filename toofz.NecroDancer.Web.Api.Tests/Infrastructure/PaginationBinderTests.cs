using System.Web.Http;
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
    class PaginationBinderTests
    {
        [TestClass]
        public class BindModel
        {
            public BindModel()
            {
                actionContext = ContextUtil.CreateActionContext();

                binder = new PaginationBinder<StubPagination>();

                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(IPagination));
                mockValueProvider = new Mock<IValueProvider>();
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = mockValueProvider.Object,
                    ModelMetadata = modelMetadata,
                };
            }

            HttpActionContext actionContext;
            const string modelName = "mockModelName";
            PaginationBinder<StubPagination> binder;
            ModelBindingContext bindingContext;
            Mock<IValueProvider> mockValueProvider;

            [TestMethod]
            public void OffsetValueIsNull_SetsModelWithDefaultOFfsetValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(StubPagination));
                var model = (StubPagination)bindingContext.Model;
                Assert.AreEqual(13, model.Offset);
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void OffsetValueIsANumber_SetsModelWithOffsetValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult(3);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(StubPagination));
                var model = (StubPagination)bindingContext.Model;
                Assert.AreEqual(3, model.Offset);
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void OffsetValueIsNotANumber_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult("ten");

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("offset", out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }

            [Ignore("Determine how to properly set up for testing validation.")]
            [TestMethod]
            public void OffsetValueIsLessThanMin_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult(int.MinValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("offset", out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }

            [Ignore("Determine how to properly set up for testing validation.")]
            [TestMethod]
            public void OffsetValueIsMoreThanMax_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("offset")).ReturnsValueProviderResult(int.MaxValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("offset", out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void LimitValueIsNull_SetsModelWithDefaultLimitValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(StubPagination));
                var model = (StubPagination)bindingContext.Model;
                Assert.AreEqual(7, model.Limit);
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void LimitValueIsANumber_SetsModelWithLimitValueAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult(3);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(StubPagination));
                var model = (StubPagination)bindingContext.Model;
                Assert.AreEqual(3, model.Limit);
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void LimitValueIsNotANumber_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult("ten");

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("limit", out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }

            [Ignore("Determine how to properly set up for testing validation.")]
            [TestMethod]
            public void LimitValueIsLessThanMin_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult(int.MinValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("limit", out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }

            [Ignore("Determine how to properly set up for testing validation.")]
            [TestMethod]
            public void LimitValueIsMoreThanMax_AddsModelErrorAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).ReturnsValueProviderResult(int.MaxValue);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue("limit", out var modelState);
                Assert.IsTrue(hasErrors);
                Assert.AreEqual(1, modelState.Errors.Count);
                Assert.IsTrue(canBind);
            }

            [TestMethod]
            public void OffsetValueAndLimitValueAreNull_SetsModelWithDefaultValuesAndReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue("limit")).Returns((ValueProviderResult)null);
                mockValueProvider.Setup(v => v.GetValue("offset")).Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(actionContext, bindingContext);

                // Assert
                Assert.IsInstanceOfType(bindingContext.Model, typeof(StubPagination));
                var model = (StubPagination)bindingContext.Model;
                Assert.AreEqual(7, model.Limit);
                Assert.AreEqual(13, model.Offset);
                Assert.IsTrue(canBind);
            }

            sealed class StubPagination : IPagination
            {
                [MinValue(0)]
                [MaxValue(30)]
                public int Offset { get; set; } = 13;
                [MinValue(1)]
                [MaxValue(20)]
                public int Limit { get; set; } = 7;
            }
        }
    }
}
