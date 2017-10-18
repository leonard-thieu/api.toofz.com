using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class LeaderboardCategoryBaseTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void CategoryIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<string> values = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new LeaderboardCategoryBaseAdapter(values);
                });
            }

            [TestMethod]
            public void CategoryIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                var values = new List<string>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new LeaderboardCategoryBaseAdapter(values);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var values = new[] { "myValue1" };

                // Act
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);

                // Assert
                Assert.IsInstanceOfType(leaderboardCategory, typeof(LeaderboardCategoryBase));
            }
        }

        [TestClass]
        public class ConvertMethod
        {
            public ConvertMethod()
            {
                values = new[] { "item1", "item2", "item3" };
                leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);
            }

            IEnumerable<string> values;
            LeaderboardCategoryBase leaderboardCategory;

            [TestMethod]
            public void ItemIsNotValid_ThrowsArgumentException()
            {
                // Arrange
                var item = "itemA";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    leaderboardCategory.Add(item);
                });
            }

            [TestMethod]
            public void ItemIsValid_AddsItem()
            {
                // Arrange
                var item = "item1";

                // Act
                leaderboardCategory.Add(item);

                // Assert
                Assert.AreEqual(1, leaderboardCategory.Count());
                Assert.IsTrue(leaderboardCategory.Contains(item));
            }
        }

        [TestClass]
        public class GetDefaults
        {
            [TestMethod]
            public void ReturnsDefaults()
            {
                // Arrange
                var values = new[] { "item1", "item2", "item3" };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);

                // Act
                leaderboardCategory.AddDefaults();

                // Assert
                CollectionAssert.AreEqual(values, leaderboardCategory.ToArray());
            }
        }

        [TestClass]
        public class GetEnumeratorMethod
        {
            [TestMethod]
            public void ReturnsEnumerator()
            {
                // Arrange
                var values = new[] { "myValue1" };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);

                // Act
                var enumerator = leaderboardCategory.GetEnumerator();

                // Assert
                Assert.IsInstanceOfType(enumerator, typeof(IEnumerator<string>));
            }
        }

        [TestClass]
        public class IEnumerable_GetEnumeratorMethod
        {
            [TestMethod]
            public void ReturnsEnumerator()
            {
                // Arrange
                var values = new[] { "myValue1" };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);
                var enumerable = (IEnumerable)leaderboardCategory;

                // Act
                var enumerator = enumerable.GetEnumerator();

                // Assert
                Assert.IsInstanceOfType(enumerator, typeof(IEnumerator));
            }
        }

        sealed class LeaderboardCategoryBaseAdapter : LeaderboardCategoryBase
        {
            public LeaderboardCategoryBaseAdapter(IEnumerable<string> values) : base(values) { }
        }
    }
}
