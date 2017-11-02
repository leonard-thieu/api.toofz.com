using System;
using System.Globalization;
using System.Web.Http.ValueProviders;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests
{
    public class ValueProviderResultExtensionsTests
    {
        public class ConvertToMethod
        {
            [Fact]
            public void ResultIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ValueProviderResult result = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    ValueProviderResultExtensions.ConvertTo<object>(result);
                });
            }

            [Fact]
            public void ValueIsConvertibleToType_ReturnsValueAsType()
            {
                // Arrange
                var result = new ValueProviderResult(15, "15", CultureInfo.InvariantCulture);

                // Act
                var value = result.ConvertTo<int>();

                // Assert
                Assert.Equal(15, value);
            }

            [Fact]
            public void ValueIsNotConvertibleToType_ReturnsDefaultOfType()
            {
                // Arrange
                var result = new ValueProviderResult(-15, "-15", CultureInfo.InvariantCulture);

                // Act
                var value = result.ConvertTo<uint>();

                // Assert
                Assert.Equal(default, value);
            }
        }
    }
}
