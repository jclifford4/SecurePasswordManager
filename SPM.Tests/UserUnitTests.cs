using Xunit;
using SPM;
using Users;
using UserUtility;

namespace SPM.Tests
{
    public class UserUnitTests
    {
        [Theory]
        [InlineData("user", "password")]
        [InlineData("user123", "password")]
        [InlineData("user", "password123")]
        [InlineData("user", "password!@#$%^&*()-_=+{}\\|,./;'<>?:\"")]
        public void CreateUser_WithValidUsernameAndPassword_ShouldSucceed(string username, string providedPassword)
        {

            // Act
            var user = new User(username, providedPassword);

            // Assert
            Assert.Equal(username, user.UserName);
            Assert.NotEqual(providedPassword, user.PasswordHash);



        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("u", "password")]
        [InlineData("us", "password")]
        [InlineData("useruseruseruseruseruseruseruseru", "password")]    // over 32 chars, (+1 of upper limit)
        public void CreateUser_WithInvalidUserNameLength_ShouldThrowArgumentException(string username, string providedPassword)
        {
            Assert.Throws<ArgumentException>(() => new User(username, providedPassword));
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
        // 139 chars, +1 over upper limit
        [InlineData("username", "012345678901234567890123456789012345678901234567890123456789"
                              + "012345678901234567890123456789012345678901234567890123456789"
                              + "012345678")]
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

        [Fact]
        public void CreateGuid_ShouldBeUnique()
        {
            // Arrange & Act
            var guid1 = UserUtil.GenerateGuidAsString();
            var guid2 = UserUtil.GenerateGuidAsString();

            // Assert
            Assert.NotEqual(guid1, guid2);
        }

        [Fact]
        public void CreateGuid_ShouldNotBeEmpty()
        {
            // Arrange & Act
            var guid1 = UserUtil.GenerateGuidAsString();

            // Assert
            Assert.NotEmpty(guid1);
        }

        [Fact]
        public void CreateGuid_ShouldHaveCorrectFormat()
        {
            // Arrange & Act
            var guid = UserUtil.GenerateGuidAsString();

            Assert.Matches(@"^[a-fA-F0-9]{8}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{12}$", guid);
        }

        [Fact]
        public void NewGuid_ShouldProduceValidGuid()
        {
            // Arrange & Act
            var guid = UserUtil.GenerateGuidAsString();

            // Assert
            Assert.True(Guid.TryParse(guid, out _));
        }

    }
}
