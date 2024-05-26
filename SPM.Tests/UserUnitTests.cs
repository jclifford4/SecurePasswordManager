using Xunit;
using SPM;
using UserAccount;

namespace SPM.Tests
{
    public class UserUnitTests
    {
        [Theory]
        [InlineData("user", "password")]
        public void CreateUser_WithValidUsernameAndPassword_SouldSucceed(string username, string providedPassword)
        {

            // Act
            var user = new User(username, providedPassword);

            // Assert
            Assert.Equal(username, user.UserName);
            Assert.NotEqual(providedPassword, user.PasswordHash);



        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("username", "")]
        [InlineData("", "")]
        [InlineData(null, "password")]
        [InlineData("username", null)]
        [InlineData(null, null)]
        public void CreateUser_WithInvalidUsernameAndPasswordWithNullOrEmpty_ShouldThrowArgumentException(string username, string providedPassword)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new User(username, providedPassword));

        }

        [Theory]
        [InlineData("username", "1")]
        [InlineData("username", "12")]
        [InlineData("username", "123")]
        [InlineData("username", "1234")]
        [InlineData("username", "12345")]
        public void CreateUser_WithInvalidPasswordLength_ShouldThrowArgumentException(string username, string providedPassword)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new User(username, providedPassword));

        }

        [Theory]
        [InlineData("username!", "password")]
        [InlineData("username?", "password")]
        [InlineData("username#", "password")]
        [InlineData("username$", "password")]
        [InlineData("username%", "password")]
        [InlineData("username^", "password")]
        [InlineData("username&", "password")]
        [InlineData("username(", "password")]
        [InlineData("username)", "password")]
        [InlineData("username<", "password")]
        [InlineData("username>", "password")]
        [InlineData("username/", "password")]
        [InlineData("username,", "password")]
        [InlineData("username.", "password")]
        [InlineData("username|", "password")]
        [InlineData("username}", "password")]
        [InlineData("username{", "password")]
        [InlineData("username\"", "password")]
        [InlineData("username\'", "password")]
        [InlineData("username:", "password")]
        [InlineData("username;", "password")]
        [InlineData("username\\", "password")]
        [InlineData("username*", "password")]
        [InlineData("username+", "password")]
        [InlineData("username=", "password")]
        public void CreateUser_WithInvalidUsername_ShouldThrowArgumentException(string username, string providedPassword)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new User(username, providedPassword));

        }
    }
}
