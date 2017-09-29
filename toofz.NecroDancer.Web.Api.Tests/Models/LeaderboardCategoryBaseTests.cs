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
                Category category = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new LeaderboardCategoryBaseAdapter(category);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var category = new Category();

                // Act
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(category);

                // Assert
                Assert.IsInstanceOfType(leaderboardCategory, typeof(LeaderboardCategoryBase));
            }
        }

        [TestClass]
        public class AddMethod
        {
            public AddMethod()
            {
                category = new Category
                {
                    ["item1"] = new CategoryItem(),
                    ["item2"] = new CategoryItem(),
                    ["item3"] = new CategoryItem(),
                };
                leaderboardCategory = new LeaderboardCategoryBaseAdapter(category);
            }

            Category category;
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
                var category = new Category
                {
                    ["item1"] = new CategoryItem(),
                    ["item2"] = new CategoryItem(),
                    ["item3"] = new CategoryItem(),
                };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(category);

                // Act
                leaderboardCategory.AddDefaults();

                // Assert
                CollectionAssert.AreEqual(category.Keys, leaderboardCategory.ToArray());
            }
        }

        [TestClass]
        public class GetEnumeratorMethod
        {
            [TestMethod]
            public void WhenCalled_ReturnsIEnumeratorOfString()
            {
                // Arrange
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(new Category());

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
            public void WhenCalled_ReturnsIEnumerator()
            {
                // Arrange
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(new Category());
                var enumerable = (IEnumerable)leaderboardCategory;

                // Act
                var enumerator = enumerable.GetEnumerator();

                // Assert
                Assert.IsInstanceOfType(enumerator, typeof(IEnumerator));
            }
        }

        sealed class LeaderboardCategoryBaseAdapter : LeaderboardCategoryBase
        {
            public LeaderboardCategoryBaseAdapter(Category category) : base(category) { }
        }
    }
}
