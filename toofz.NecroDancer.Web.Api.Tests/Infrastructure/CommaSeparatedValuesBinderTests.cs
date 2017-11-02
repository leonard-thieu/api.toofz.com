using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Moq;
using toofz.NecroDancer.Web.Api.Infrastructure;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class CommaSeparatedValuesBinderTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var binder = new CommaSeparatedValuesBinderAdapter();

                // Assert
                Assert.IsAssignableFrom<CommaSeparatedValuesBinder<string>>(binder);
            }

            sealed class CommaSeparatedValuesBinderAdapter : CommaSeparatedValuesBinder<string>
            {
                protected override CommaSeparatedValues<string> GetModel() => throw new NotImplementedException();
            }
        }

        public class BindModelMethod
        {
            public BindModelMethod()
            {
                var values = new[] { "item1", "item2", "item3" };
                var model = new CommaSeparatedValuesAdapter(values);
                binder = new CommaSeparatedValuesBinderAdapter(model);

                var data = new EmptyModelMetadataProvider();
                var modelMetadata = data.GetMetadataForType(null, typeof(CommaSeparatedValues<string>));
                mockValueProvider = new Mock<IValueProvider>();
                var valueProvider = mockValueProvider.Object;
                bindingContext = new ModelBindingContext
                {
                    ModelName = modelName,
                    ValueProvider = valueProvider,
                    ModelMetadata = modelMetadata,
                };
            }

            private string modelName = "myModelName";
            private CommaSeparatedValuesBinder<string> binder;
            private ModelBindingContext bindingContext;
            private Mock<IValueProvider> mockValueProvider;

            [Fact]
            public void ValueIsNull_SetsModelWithDefaultValues()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).Returns((ValueProviderResult)null);

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsAssignableFrom<CommaSeparatedValues<string>>(bindingContext.Model);
                var model = (CommaSeparatedValues<string>)bindingContext.Model;
                var expected = new[] { "item1", "item2", "item3" };
                var actual = model.ToArray();
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ValueIsNull_ReturnsTrue()
            {
                // Arrange
                mockValueProvider
                    .Setup(v => v.GetValue(modelName))
                    .Returns((ValueProviderResult)null);

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.True(canBind);
            }

            [Fact]
            public void ValueIsValidCommaSeparatedValues_SetsModelWithValues()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,item3");

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                Assert.IsAssignableFrom<CommaSeparatedValues<string>>(bindingContext.Model);
                var model = (CommaSeparatedValues<string>)bindingContext.Model;
                var expected = new[] { "item1", "item3" };
                var actual = model.ToArray();
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ValueIsValidCommaSeparatedValues_ReturnsTrue()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,item3");

                // Act
                var canBind = binder.BindModel(null, bindingContext);

                // Assert
                Assert.True(canBind);
            }

            [Fact]
            public void ValueContainsInvalidValues_AddsModelErrorsForInvalidValues()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,itemA,item3");

                // Act
                binder.BindModel(null, bindingContext);

                // Assert
                var hasErrors = bindingContext.ModelState.TryGetValue(modelName, out var modelState);
                Assert.True(hasErrors);
                Assert.Equal(1, modelState.Errors.Count);
            }

            [Fact]
            public void ValueContainsInvalidValues_ReturnsFalse()
            {
                // Arrange
                mockValueProvider.Setup(v => v.GetValue(modelName)).ReturnsValueProviderResult("item1,itemA,item3");

                // Act
                var success = binder.BindModel(null, bindingContext);

                // Assert
                Assert.False(success);
            }

            private sealed class CommaSeparatedValuesBinderAdapter : CommaSeparatedValuesBinder<string>
            {
                public CommaSeparatedValuesBinderAdapter(CommaSeparatedValues<string> model)
                {
                    this.model = model;
                }

                private readonly CommaSeparatedValues<string> model;

                protected override CommaSeparatedValues<string> GetModel() => model;
            }

            private sealed class CommaSeparatedValuesAdapter : CommaSeparatedValues<string>
            {
                public CommaSeparatedValuesAdapter(IEnumerable<string> defaults)
                {
                    this.defaults = defaults;
                }

                private readonly IEnumerable<string> defaults;

                protected override string Convert(string item)
                {
                    if (!defaults.Contains(item))
                        throw new ArgumentException();

                    return item;
                }

                protected override IEnumerable<string> GetDefaults() => defaults;
            }
        }
    }
}
