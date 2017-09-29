using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class PlayersSortParamsTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var sort = new PlayersSortParams();

                // Assert
                Assert.IsInstanceOfType(sort, typeof(PlayersSortParams));
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
                var sort = new PlayersSortParams();

                // Act
                sort.Add(item);

                // Assert
                var item2 = sort.First();
                Assert.AreEqual(item, item2);
            }

            [TestMethod]
            public void ItemIsInvalid_ThrowsArgumentException()
            {
                // Arrange
                var sort = new PlayersSortParams();
                var item = "myInvalidItem";

                // Act -> Assert
                var ex = Assert.ThrowsException<ArgumentException>(() =>
                {
                    sort.Add(item);
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
                var sort = new PlayersSortParams();

                // Act
                sort.AddDefaults();

                // Assert
                var expected = new[] { "-entries", "display_name", "id" };
                var actual = sort.ToArray();
                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
