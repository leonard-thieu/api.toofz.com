using toofz.NecroDancer.Web.Api.Identity;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Identity
{
    public class ApplicationDbContextTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var db = new ApplicationDbContext();

                // Assert
                Assert.IsAssignableFrom<ApplicationDbContext>(db);
            }
        }

        public class Constructor_String
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var connectionString = "Data Source=localhost;Integrated Security=SSPI";

                // Act
                var db = new ApplicationDbContext(connectionString);

                // Assert
                Assert.IsAssignableFrom<ApplicationDbContext>(db);
            }
        }
    }
}
