using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.NecroDancer.Web.Api.Models;

namespace toofz.NecroDancer.Web.Api.Tests.Models
{
    class CommaSeparatedValuesTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var csv = new CommaSeparatedValuesAdapter();

                // Assert
                Assert.IsInstanceOfType(csv, typeof(CommaSeparatedValues<string>));
            }
        }

        [TestClass]
        public class AddMethod
        {
            [TestMethod]
            public void AddsItem()
            {
                // Arrange
                var csv = new CommaSeparatedValuesAdapter();
                var item = "myItem";

                // Act
                csv.Add(item);

                // Assert
                var item2 = csv.First();
                Assert.AreEqual(item, item2);
            }
        }

        [TestClass]
        public class AddDefaultsMethod
        {
            [TestMethod]
            public void AddsDefaults()
            {
                // Arrange
                var defaults = new[] { "myItem1", "myItem2" };
                var csv = new CommaSeparatedValuesAdapter(defaults);

                // Act
                csv.AddDefaults();

                // Assert
                var actual = csv.ToArray();
                CollectionAssert.AreEqual(defaults, actual);
            }
        }

        [TestClass]
        public class GetEnumeratorMethod
        {
            [TestMethod]
            public void ReturnsEnumerator()
            {
                // Arrange
                var csv = new CommaSeparatedValuesAdapter();

                // Act
                var enumerator = csv.GetEnumerator();

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
                var csv = new CommaSeparatedValuesAdapter();
                var enumerable = (IEnumerable)csv;

                // Act
                var enumerator = enumerable.GetEnumerator();

                // Assert
                Assert.IsInstanceOfType(enumerator, typeof(IEnumerator));
            }
        }

        sealed class CommaSeparatedValuesAdapter : CommaSeparatedValues<string>
        {
            public CommaSeparatedValuesAdapter() { }

            public CommaSeparatedValuesAdapter(IEnumerable<string> defaults)
            {
                this.defaults = defaults;
            }

            readonly IEnumerable<string> defaults;

            protected override string Convert(string item) => item;

            protected override IEnumerable<string> GetDefaults() => defaults;
        }
    }
}
