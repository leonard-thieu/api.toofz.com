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
            [DisplayFact(nameof(ArgumentNullException))]
            public void ResultIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                ValueProviderResult result = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    result.ConvertTo<object>();
                });
            }

            [DisplayFact]
            public void ValueIsConvertibleToType_ReturnsValueAsType()
            {
                // Arrange
                var result = new ValueProviderResult(15, "15", CultureInfo.InvariantCulture);

                // Act
                var value = result.ConvertTo<int>();

                // Assert
                Assert.Equal(15, value);
            }

            [DisplayFact]
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
