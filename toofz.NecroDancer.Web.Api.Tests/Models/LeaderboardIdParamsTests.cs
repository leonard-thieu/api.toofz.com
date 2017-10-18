using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class LeaderboardIdParamsTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var lbids = new LeaderboardIdParams();

                // Assert
                Assert.IsInstanceOfType(lbids, typeof(LeaderboardIdParams));
            }
        }

        [TestClass]
        public class ConvertMethod
        {
            [TestMethod]
            public void ItemIsValid_AddsConvertedItem()
            {
                // Arrange
                var lbids = new LeaderboardIdParams();
                var item = "2047616";

                // Act
                lbids.Add(item);

                // Assert
                var item2 = lbids.First();
                Assert.AreEqual(2047616, item2);
            }

            [TestMethod]
            public void ItemIsInvalid_ThrowsException()
            {
                // Arrange
                var lbids = new LeaderboardIdParams();
                var item = "a";

                // Act -> Assert
                Assert.ThrowsException<FormatException>(() =>
                {
                    lbids.Add(item);
                });
            }
        }

        [TestClass]
        public class GetDefaultsMethod
        {
            [TestMethod]
            public void ReturnsDefaults()
            {
                // Arrange
                var lbids = new LeaderboardIdParams();

                // Act
                lbids.AddDefaults();

                // Assert
                Assert.IsFalse(lbids.Any());
            }
        }
    }
}
