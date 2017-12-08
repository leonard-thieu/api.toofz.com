using System.Collections;
using System.Collections.Generic;
using System.Linq;
using toofz.NecroDancer.Web.Api.Models;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    public class CommaSeparatedValuesTests
    {
        public class Constructor
        {
            [DisplayFact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var csv = new CommaSeparatedValuesAdapter();

                // Assert
                Assert.IsAssignableFrom<CommaSeparatedValues<string>>(csv);
            }
        }

        public class AddMethod
        {
            [DisplayFact]
            public void AddsItem()
            {
                // Arrange
                var csv = new CommaSeparatedValuesAdapter();
                var item = "myItem";

                // Act
                csv.Add(item);

                // Assert
                var item2 = csv.First();
                Assert.Equal(item, item2);
            }
        }

        public class AddDefaultsMethod
        {
            [DisplayFact]
            public void AddsDefaults()
            {
                // Arrange
                var defaults = new[] { "myItem1", "myItem2" };
                var csv = new CommaSeparatedValuesAdapter(defaults);

                // Act
                csv.AddDefaults();

                // Assert
                var actual = csv.ToArray();
                Assert.Equal(defaults, actual);
            }
        }

        public class GetEnumeratorMethod
        {
            [DisplayFact]
            public void ReturnsEnumerator()
            {
                // Arrange
                var csv = new CommaSeparatedValuesAdapter();

                // Act
                var enumerator = csv.GetEnumerator();

                // Assert
                Assert.IsAssignableFrom<IEnumerator<string>>(enumerator);
            }
        }

        public class IEnumerable_GetEnumeratorMethod
        {
            [DisplayFact]
            public void ReturnsEnumerator()
            {
                // Arrange
                var csv = new CommaSeparatedValuesAdapter();
                var enumerable = (IEnumerable)csv;

                // Act
                var enumerator = enumerable.GetEnumerator();

                // Assert
                Assert.IsAssignableFrom<IEnumerator>(enumerator);
            }
        }

        private sealed class CommaSeparatedValuesAdapter : CommaSeparatedValues<string>
        {
            public CommaSeparatedValuesAdapter() { }

            public CommaSeparatedValuesAdapter(IEnumerable<string> defaults)
            {
                this.defaults = defaults;
            }

            private readonly IEnumerable<string> defaults;

            protected override string Convert(string item) => item;

            protected override IEnumerable<string> GetDefaults() => defaults;
        }
    }
}
