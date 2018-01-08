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
            [DisplayFact(nameof(LeaderboardIdParams))]
            public void ReturnsLeaderboardIdParams()
            {
                // Arrange -> Act
                var leaderboardIdParams = new LeaderboardIdParams();

                // Assert
                Assert.IsAssignableFrom<LeaderboardIdParams>(leaderboardIdParams);
            }
        }

        public class ConvertMethod
        {
            [DisplayFact(nameof(Int32))]
            public void LeaderboardIdIsValid_AddsLeaderboardIdAsInt32()
            {
                // Arrange
                var leaderboardIdParams = new LeaderboardIdParams();
                var leaderboardId = "2047616";

                // Act
                leaderboardIdParams.Add(leaderboardId);

                // Assert
                Assert.Equal(2047616, leaderboardIdParams.Last());
            }

            [DisplayFact(nameof(FormatException))]
            public void LeaderboardIdIsInvalid_ThrowsFormatException()
            {
                // Arrange
                var leaderboardIdParams = new LeaderboardIdParams();
                var leaderboardId = "a";

                // Act -> Assert
                Assert.Throws<FormatException>(() =>
                {
                    leaderboardIdParams.Add(leaderboardId);
                });
            }
        }

        public class GetDefaultsMethod
        {
            [DisplayFact]
            public void ReturnsDefaults()
            {
                // Arrange
                var leaderboardIdParams = new LeaderboardIdParams();

                // Act
                leaderboardIdParams.AddDefaults();

                // Assert
                Assert.Empty(leaderboardIdParams);
            }
        }
    }
}
