using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class PlayersSortParamTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var playersSortParam = new PlayersSortParam();

                // Assert
                Assert.IsInstanceOfType(playersSortParam, typeof(PlayersSortParam));
            }
        }

        [TestClass]
        public class AddMethod
        {
            [DataTestMethod]
            [DataRow("id")]
            [DataRow("display_name")]
            [DataRow("updated_at")]
            [DataRow("entries")]
            [DataRow("-id")]
            [DataRow("-display_name")]
            [DataRow("-updated_at")]
            [DataRow("-entries")]
            public void ItemIsValid_AddsItem(string item)
            {
                // Arrange
                var playersSortParam = new PlayersSortParam();

                // Act
                playersSortParam.Add(item);

                // Assert
                var item2 = playersSortParam.First();
                Assert.AreEqual(item, item2);
            }

            [TestMethod]
            public void ItemIsInvalid_ThrowsArgumentException()
            {
                // Arrange
                var playersSortParam = new PlayersSortParam();
                var item = "myInvalidItem";

                // Act -> Assert
                var ex = Assert.ThrowsException<ArgumentException>(() =>
                {
                    playersSortParam.Add(item);
                });
                Assert.IsNull(ex.ParamName);
            }
        }

        [TestClass]
        public class GetDefaultsMethod
        {
            [TestMethod]
            public void ReturnsDefaults()
            {
                // Arrange
                var playersSortParam = new PlayersSortParam();

                // Act
                playersSortParam.AddDefaults();

                // Assert
                var expected = new[] { "-entries", "display_name", "id" };
                var actual = playersSortParam.ToArray();
                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
