using Xunit;
using UserRepository;
using UserAccount;

namespace UserRepositoryTests
{
    public class UserRepositoryIntegrationTests
    {

        [Fact]
        public void CanOpenAndCloseConnection_ResultSuccess()
        {
            var userRepositoryAcessor = new UserRepositoryAcessor();

            bool isConnected = userRepositoryAcessor.OpenDatabaseConnection();
            bool isClosed = userRepositoryAcessor.CloseDatabaseConnection();

            Assert.True(isConnected);
            Assert.True(isClosed);
        }

        [Fact]
        public void AddUserToDatabase_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser", "testhash");

            // Act
            bool isAdded = userRepositoryAcessor.Add(user);

            // Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void AddDuplicateUserToDatabase_ReturnFailed()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser", "testhash");

            // Act
            bool isAdded = userRepositoryAcessor.Add(user);
            bool isAddedAgain = userRepositoryAcessor.Add(user);

            // Assert
            Assert.True(isAdded);
            Assert.False(isAddedAgain);
        }
    }
}
