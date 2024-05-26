using Xunit;
using SPM;
using UserAccount;
using UserUtility;
using HashUtility;

namespace SPM.Tests
{
    public class UserIntegrationTests
    {

        [Fact]
        public void HashPassword_WithValidUsernameAndPasswordThenHashPasswordOutputString84Bits_ReturnSuccess()
        {
            // Arrange
            string username = "testuser1";
            string password = "testpassword1";

            // Act
            string hash = UserUtil.HashPassword(username, password);
            int hashLength = hash.Length;

            // Assert
            Assert.NotEqual(password, hash);
            Assert.True(hashLength == 84);
        }

        [Fact]
        public void CreateUser_WithValidUsernameAndPassword_ReturnSuccess()
        {
            // Arrange
            User user;
            string userName = "testuser2";
            string password = "testpassword2";

            // Act
            user = UserUtil.CreateUserAndHashPassword(userName, password);

            // Assert
            Assert.NotEqual(user.PasswordHash, password);
            Assert.NotEmpty(user.PasswordHash);

        }



    }
}
