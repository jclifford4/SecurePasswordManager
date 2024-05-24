using Xunit;
using SPM;
using UserRepository;
using UserAccount;

namespace SPM.Tests
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
            var user = new User("testuser1", "testhash1");

            // Act
            bool isAdded = userRepositoryAcessor.Add(user);
            bool isAddedAgain = userRepositoryAcessor.Add(user);

            // Assert
            Assert.True(isAdded);
            Assert.False(isAddedAgain);
        }

        [Fact]
        public void CheckUserNameExists_ReturnFailed()
        {
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser2", "testhash2");

            bool exists = userRepositoryAcessor.UsernameExists(user.UserName);

            Assert.False(exists);
        }

        [Fact]
        public void CheckUserNameExists_ReturnSuccess()
        {
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser3", "testhash3");

            bool isAdded = userRepositoryAcessor.Add(user);
            bool exists = userRepositoryAcessor.UsernameExists(user.UserName);

            Assert.True(isAdded);
            Assert.True(exists);
        }
        [Fact]
        public void AddThenDeleteUser_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser4", "testhash4");

            // Act
            bool isAdded = userRepositoryAcessor.Add(user);
            bool exists = userRepositoryAcessor.UsernameExists(user.UserName);
            bool isDeleted = userRepositoryAcessor.Delete(user);

            // Assert
            Assert.True(isAdded);
            Assert.True(exists);
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteAllFromTable_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user1 = new User("testuser5", "testhash5");
            var user2 = new User("testuser6", "testhash6");
            var user3 = new User("testuser7", "testhash7");
            var user4 = new User("testuser8", "testhash8");

            // Act
            userRepositoryAcessor.Add(user1);
            userRepositoryAcessor.Add(user2);
            userRepositoryAcessor.Add(user3);
            userRepositoryAcessor.Add(user4);
            bool allDeleted = userRepositoryAcessor.DeleteAll();

            // Assert
            Assert.True(allDeleted);
        }

        [Fact]
        public void AddUserAndCount_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser9", "testhash9");

            // Act
            bool isAdded = userRepositoryAcessor.Add(user);
            int count = userRepositoryAcessor.Count();

            // Assert
            Assert.True(isAdded);
            Assert.Equal(1, count);


        }
    }
}
