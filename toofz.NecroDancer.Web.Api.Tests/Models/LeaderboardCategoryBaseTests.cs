using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    public class LeaderboardCategoryBaseTests
    {
        public class Constructor
        {
            [Fact]
            public void CategoryIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                IEnumerable<string> values = null;

                // Act -> Assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new LeaderboardCategoryBaseAdapter(values);
                });
            }

            [Fact]
            public void CategoryIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                var values = new List<string>();

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    new LeaderboardCategoryBaseAdapter(values);
                });
            }

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var values = new[] { "myValue1" };

                // Act
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);

                // Assert
                Assert.IsAssignableFrom<LeaderboardCategoryBase>(leaderboardCategory);
            }
        }

        public class ConvertMethod
        {
            public ConvertMethod()
            {
                values = new[] { "item1", "item2", "item3" };
                leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);
            }

            private IEnumerable<string> values;
            private LeaderboardCategoryBase leaderboardCategory;

            [Fact]
            public void ItemIsNotValid_ThrowsArgumentException()
            {
                // Arrange
                var item = "itemA";

                // Act -> Assert
                Assert.Throws<ArgumentException>(() =>
                {
                    leaderboardCategory.Add(item);
                });
            }

            [Fact]
            public void ItemIsValid_AddsItem()
            {
                // Arrange
                var item = "item1";

                // Act
                leaderboardCategory.Add(item);

                // Assert
                Assert.Equal(1, leaderboardCategory.Count());
                Assert.Contains(item, leaderboardCategory);
            }
        }

        public class GetDefaults
        {
            [Fact]
            public void ReturnsDefaults()
            {
                // Arrange
                var values = new[] { "item1", "item2", "item3" };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);

                // Act
                leaderboardCategory.AddDefaults();

                // Assert
                Assert.Equal(values, leaderboardCategory.ToArray());
            }
        }

        public class GetEnumeratorMethod
        {
            [Fact]
            public void ReturnsEnumerator()
            {
                // Arrange
                var values = new[] { "myValue1" };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);

                // Act
                var enumerator = leaderboardCategory.GetEnumerator();

                // Assert
                Assert.IsAssignableFrom<IEnumerator<string>>(enumerator);
            }
        }

        public class IEnumerable_GetEnumeratorMethod
        {
            [Fact]
            public void ReturnsEnumerator()
            {
                // Arrange
                var values = new[] { "myValue1" };
                var leaderboardCategory = new LeaderboardCategoryBaseAdapter(values);
                var enumerable = (IEnumerable)leaderboardCategory;

                // Act
                var enumerator = enumerable.GetEnumerator();

                // Assert
                Assert.IsAssignableFrom<IEnumerator>(enumerator);
            }
        }

        private sealed class LeaderboardCategoryBaseAdapter : LeaderboardCategoryBase
        {
            public LeaderboardCategoryBaseAdapter(IEnumerable<string> values) : base(values) { }
        }
    }
}
