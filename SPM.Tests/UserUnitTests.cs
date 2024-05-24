using Xunit;
using SPM;
using UserAccount;


namespace SPM.Tests
{
    public class UserUnitTests
    {
        [Fact]
        public void UserConstructor_InitializeProperties_ReturnSuccess()
        {
            // Arange
            var username = "testuser";
            var hash = "somehash";

            // Act
            User user = new User(username, hash);

            // Assert
            Assert.Equal(username, user.UserName);
            Assert.Equal(hash, user.PasswordHash);

        }

        [Fact]
        public void UserConstructor_InitializeEmpty_ReturnSuccess()
        {
            // Arange
            string empty = string.Empty;

            // Act
            User user = new User();

            // Assert
            Assert.Equal(empty, user.UserName);
            Assert.Equal(empty, user.PasswordHash);
        }
    }
}
