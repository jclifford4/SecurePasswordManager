using Xunit;
using SPM;
using DBServer;

namespace SPM.Tests
{
    public class DBServerTests
    {
        [Fact]
        public void CreateServer_ShouldSucceed()
        {
            // Arrange 
            string dbname = "testdb";
            string testuser = "testuser";
            string testhash = "testhash";
            string testhost = "localhost";

            // Act
            Server server = new Server(dbname, testuser, testhash);

            // Assert
            Assert.Equal(dbname, server.DatabaseName);
            Assert.Equal(testhost, server.Hostname);
            Assert.Equal(testuser, server.User);
            Assert.Equal(testhash, server.PasswordHash);

        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData(null, "", "")]
        [InlineData(null, null, "")]
        [InlineData("", null, "")]
        [InlineData("", "", null)]
        [InlineData("", null, null)]
        [InlineData(null, "", null)]
        [InlineData(null, null, null)]
        [InlineData("testdb", null, "testhash")]
        [InlineData("testdb", "testuser", null)]
        [InlineData(null, "testuser", "testhash")]
        public void CreateServer_BlankOrNullData_ShouldThrowException(string dbname, string user, string hash)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Server(dbname, user, hash));

        }
    }
}
