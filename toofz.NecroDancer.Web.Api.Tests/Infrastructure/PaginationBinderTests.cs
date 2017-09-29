using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    class PaginationBinderTests
    {
        [TestClass]
        public class BindModel
        {
            HttpActionContext actionContext;
            const string modelName = "mockModelName";
            PaginationBinder<StubPagination> binder;
            ModelBindingContext bindingContext;
            Mock<IValueProvider> mock_ValueProvider;

            [TestInitialize]
            public void Initialize()
            {
                actionContext = ContextUtil.CreateActionContext();

                binder = new PaginationBinder<StubPagination>();

                var data = new DataAnnotationsModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(IPagination));
                mock_ValueProvider = new Mock<IValueProvider>();
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = mock_ValueProvider.Object,
                    ModelMetadata = modelMetadata,
                };
            }

            [TestMethod]
            public void OffsetValueIsNull_SetsModelWithDefaultOFfsetValueAndReturnsTrue()
            {
                // Arrange
                mock_ValueProvider
                    .Setup(v => v.GetValue("offset"))
                    .Returns((ValueProviderResult)null);

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("offset"))
                    .Returns(new ValueProviderResult(3, null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("offset"))
                    .Returns(new ValueProviderResult("ten", null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("offset"))
                    .Returns(new ValueProviderResult(int.MinValue, null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("offset"))
                    .Returns(new ValueProviderResult(int.MaxValue, null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("limit"))
                    .Returns((ValueProviderResult)null);

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("limit"))
                    .Returns(new ValueProviderResult(3, null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("limit"))
                    .Returns(new ValueProviderResult("ten", null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("limit"))
                    .Returns(new ValueProviderResult(int.MinValue, null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("limit"))
                    .Returns(new ValueProviderResult(int.MaxValue, null, null));

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
                mock_ValueProvider
                    .Setup(v => v.GetValue("limit"))
                    .Returns((ValueProviderResult)null);
                mock_ValueProvider
                    .Setup(v => v.GetValue("offset"))
                    .Returns((ValueProviderResult)null);

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
