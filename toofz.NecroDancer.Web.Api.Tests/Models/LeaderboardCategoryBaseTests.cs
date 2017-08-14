using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Leaderboards;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class LeaderboardCategoryBaseTests
    {
        [TestClass]
        public class Add
        {
            Category category;
            LeaderboardCategoryBase leaderboardCategory;

            [TestInitialize]
            public void Initialize()
            {
                category = new Category
                {
                    { "item1", new CategoryItem() },
                    { "item2", new CategoryItem() },
                    { "item3", new CategoryItem() },
                };
                leaderboardCategory = new MockLeaderboardCategoryBase(category);
            }

            [TestMethod]
            public void ItemIsNotValid_ThrowsArgumentException()
            {
                // Arrange
                var item = "itemA";

                // Act
                var ex = Record.Exception(() =>
                {
                    leaderboardCategory.Add(item);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
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
        public class AddAll
        {
            Category category;
            LeaderboardCategoryBase leaderboardCategory;

            [TestInitialize]
            public void Initialize()
            {
                category = new Category
                {
                    { "item1", new CategoryItem() },
                    { "item2", new CategoryItem() },
                    { "item3", new CategoryItem() },
                };
                leaderboardCategory = new MockLeaderboardCategoryBase(category);
            }

            [TestMethod]
            public void WhenCalled_AddsAllItems()
            {
                // Arrange -> Act
                leaderboardCategory.AddAll();

                // Assert
                Assert.AreEqual(3, leaderboardCategory.Count());
            }
        }

        [TestClass]
        public class GetEnumerator
        {
            [TestMethod]
            public void WhenCalled_ReturnsIEnumeratorOfString()
            {
                // Arrange
                var leaderboardCategory = new MockLeaderboardCategoryBase(new Category());

                // Act
                var enumerator = leaderboardCategory.GetEnumerator();

                // Assert
                Assert.IsInstanceOfType(enumerator, typeof(IEnumerator<string>));
            }
        }

        [TestClass]
        public class IEnumerableGetEnumerator
        {
            [TestMethod]
            public void WhenCalled_ReturnsIEnumerator()
            {
                // Arrange
                var leaderboardCategory = new MockLeaderboardCategoryBase(new Category());
                var enumerable = leaderboardCategory as IEnumerable;

                // Act
                var enumerator = enumerable.GetEnumerator();

                // Assert
                Assert.IsInstanceOfType(enumerator, typeof(IEnumerator));
            }
        }
    }
}
