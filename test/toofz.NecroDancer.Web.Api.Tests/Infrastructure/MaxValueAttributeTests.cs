using toofz.NecroDancer.Web.Api.Infrastructure;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Infrastructure
{
    public class MaxValueAttributeTests
    {
        public class IsValid
        {
            [DisplayFact]
            public void GreaterThanMax_ReturnsFalse()
            {
                // Arrange
                var maxValueAttribute = new MaxValueAttribute(5);
                var value = 6;

                // Act
                var isValid = maxValueAttribute.IsValid(value);

                // Assert
                Assert.False(isValid);
            }

            [DisplayTheory]
            [InlineData(5)]
            [InlineData(1)]
            public void LessThanOrEqualMax_ReturnsTrue(int value)
            {
                // Arrange
                var maxValueAttribute = new MaxValueAttribute(5);

                // Act
                var isValid = maxValueAttribute.IsValid(value);

                // Assert
                Assert.True(isValid);
            }
        }
    }
}
