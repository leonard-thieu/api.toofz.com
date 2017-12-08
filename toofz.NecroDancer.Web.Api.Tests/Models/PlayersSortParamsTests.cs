using System;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    public class PlayersSortParamsTests
    {
        public class Constructor
        {
            [DisplayFact(nameof(PlayersSortParams))]
            public void ReturnsPlayersSortParams()
            {
                // Arrange -> Act
                var sort = new PlayersSortParams();

                // Assert
                Assert.IsAssignableFrom<PlayersSortParams>(sort);
            }
        }

        public class ConvertMethod
        {
            [DisplayTheory]
            [InlineData("id")]
            [InlineData("display_name")]
            [InlineData("updated_at")]
            [InlineData("entries")]
            [InlineData("-id")]
            [InlineData("-display_name")]
            [InlineData("-updated_at")]
            [InlineData("-entries")]
            public void PropertyIsValid_AddsProperty(string property)
            {
                // Arrange
                var sort = new PlayersSortParams();

                // Act
                sort.Add(property);

                // Assert
                Assert.Equal(property, sort.Last());
            }

            [DisplayFact(nameof(ArgumentException))]
            public void PropertyIsInvalid_ThrowsArgumentException()
            {
                // Arrange
                var sort = new PlayersSortParams();
                var property = "myInvalidProperty";

                // Act -> Assert
                var ex = Assert.Throws<ArgumentException>(() =>
                {
                    sort.Add(property);
                });
                Assert.Null(ex.ParamName);
            }
        }

        public class GetDefaultsMethod
        {
            [DisplayFact]
            public void ReturnsDefaults()
            {
                // Arrange
                var sort = new PlayersSortParams();

                // Act
                sort.AddDefaults();

                // Assert
                Assert.Equal(new[] { "-entries", "display_name", "id" }, sort);
            }
        }
    }
}
