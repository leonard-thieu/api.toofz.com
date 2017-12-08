using toofz.NecroDancer.Web.Api.Infrastructure;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class MinValueAttributeTests
    {
        public class IsValid
        {
            [DisplayFact]
            public void LessThanMin_ReturnsFalse()
            {
                // Arrange
                var minValueAttribute = new MinValueAttribute(5);
                var value = -1;

                // Act
                var isValid = minValueAttribute.IsValid(value);

                // Assert
                Assert.False(isValid);
            }

            [DisplayTheory]
            [InlineData(5)]
            [InlineData(32)]
            public void GreaterThanOrEqualMin_ReturnsTrue(int value)
            {
                // Arrange
                var minValueAttribute = new MinValueAttribute(5);

                // Act
                var isValid = minValueAttribute.IsValid(value);

                // Assert
                Assert.True(isValid);
            }
        }
    }
}
