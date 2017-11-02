using System;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    public class LeaderboardIdParamsTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var lbids = new LeaderboardIdParams();

                // Assert
                Assert.IsAssignableFrom<LeaderboardIdParams>(lbids);
            }
        }

        public class ConvertMethod
        {
            [Fact]
            public void ItemIsValid_AddsConvertedItem()
            {
                // Arrange
                var lbids = new LeaderboardIdParams();
                var item = "2047616";

                // Act
                lbids.Add(item);

                // Assert
                var item2 = lbids.First();
                Assert.Equal(2047616, item2);
            }

            [Fact]
            public void ItemIsInvalid_Throws()
            {
                // Arrange
                var lbids = new LeaderboardIdParams();
                var item = "a";

                // Act -> Assert
                Assert.Throws<FormatException>(() =>
                {
                    lbids.Add(item);
                });
            }
        }

        public class GetDefaultsMethod
        {
            [Fact]
            public void ReturnsDefaults()
            {
                // Arrange
                var lbids = new LeaderboardIdParams();

                // Act
                lbids.AddDefaults();

                // Assert
                Assert.False(lbids.Any());
            }
        }
    }
}
