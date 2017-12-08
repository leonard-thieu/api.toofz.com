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
            [DisplayFact]
            public void ReturnsInstance()
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
            public void ItemIsValid_AddsItem(string item)
            {
                // Arrange
                var sort = new PlayersSortParams();

                // Act
                sort.Add(item);

                // Assert
                var item2 = sort.First();
                Assert.Equal(item, item2);
            }

            [DisplayFact(nameof(ArgumentException))]
            public void ItemIsInvalid_ThrowsArgumentException()
            {
                // Arrange
                var sort = new PlayersSortParams();
                var item = "myInvalidItem";

                // Act -> Assert
                var ex = Assert.Throws<ArgumentException>(() =>
                {
                    sort.Add(item);
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
                var expected = new[] { "-entries", "display_name", "id" };
                var actual = sort.ToArray();
                Assert.Equal(expected, actual);
            }
        }
    }
}
