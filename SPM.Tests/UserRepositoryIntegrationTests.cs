using Xunit;
using SPM;
using UserRepository;
using UserAccount;


namespace SPM.Tests
{
    [Collection("SequentialTests")]
    public class UserRepositoryIntegrationTests
    {

        public UserRepositoryIntegrationTests()
        {
            // Set environment variables for the test run
            Environment.SetEnvironmentVariable("ENCRYPTION_KEY", "+BMnIcKdA/q8ie2jlP3sCmjni9dEAWiYGZyn7gNPa3A=");
            Environment.SetEnvironmentVariable("HOST", "localhost");
            Environment.SetEnvironmentVariable("USER", "testing");
            Environment.SetEnvironmentVariable("PASSWORD", "bigpassword");
            Environment.SetEnvironmentVariable("DATABASE", "spmdb");
            Environment.SetEnvironmentVariable("BACKUP_PATH", "J:\\dotnetProjects\\SecurePasswordManager\\SPM\\SPMDatabase\\backups\\");
        }

        /// <summary>
        /// internal function to set environment variables to be able to restore
        /// the initial state of the DB for each test.
        /// </summary>
        /// <returns>true or false</returns>
        internal static bool RestoreDB()
        {
            var userRepositoryAcessor = new UserRepositoryAcessor();

            string host = Environment.GetEnvironmentVariable("HOST");
            string user = Environment.GetEnvironmentVariable("USER");
            string password = Environment.GetEnvironmentVariable("PASSWORD");
            string database = Environment.GetEnvironmentVariable("DATABASE");
            string backupPath = Environment.GetEnvironmentVariable("BACKUP_PATH");
            string fileName = "MySqlBackup_2024-05-25_16-46-25-4625.sql";
            return userRepositoryAcessor.Restore(host, user, password, database, backupPath, fileName);

        }


        [Fact]
        public void CanOpenAndCloseConnection_ResultSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            bool isConnected = userRepositoryAcessor.OpenDatabaseConnection();
            bool isClosed = userRepositoryAcessor.CloseDatabaseConnection();

            // Assert
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
            // Restore DB
            bool isRestored = RestoreDB();

            // Assert
            Assert.True(isAdded);
            Assert.True(isRestored);

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
            // Restore DB
            bool isRestored = RestoreDB();

            // Assert
            Assert.True(isAdded);
            Assert.False(isAddedAgain);
            Assert.True(isRestored);
        }

        [Fact]
        public void CheckUserNameExists_ReturnFailed()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser2", "testhash2");

            // Act
            bool exists = userRepositoryAcessor.UsernameExists(user.UserName);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void CheckUserNameExists_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser3", "testhash3");

            // Act
            bool isAdded = userRepositoryAcessor.Add(user);
            bool exists = userRepositoryAcessor.UsernameExists(user.UserName);

            // Assert
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

            // Restore DB
            bool isRestored = RestoreDB();

            // Assert
            Assert.True(isAdded);
            Assert.True(exists);
            Assert.True(isDeleted);
            Assert.True(isRestored);
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

            // Restore DB
            bool isRestored = RestoreDB();

            // Assert
            Assert.True(allDeleted);
            Assert.True(isRestored);
        }

        [Fact]
        public void AddUserAndCount_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            var user = new User("testuser9", "testhash9");

            // Act
            int initialCount = userRepositoryAcessor.Count();
            bool isAdded = userRepositoryAcessor.Add(user);
            int currentCount = userRepositoryAcessor.Count();

            // Restore DB
            bool isRestored = RestoreDB();

            // Assert
            Assert.True(isAdded);
            Assert.True(currentCount > initialCount);     // Initial + newly added user
            Assert.True(isRestored);

        }

        [Fact]
        public void GetDatabaseBackups_CountPlusOne_ReturnSuccess()
        {
            // Arrange
            string host = Environment.GetEnvironmentVariable("HOST");
            string user = Environment.GetEnvironmentVariable("USER");
            string password = Environment.GetEnvironmentVariable("PASSWORD");
            string database = Environment.GetEnvironmentVariable("DATABASE");
            string backupPath = Environment.GetEnvironmentVariable("BACKUP_PATH");

            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int initialBackupFileCount = userRepositoryAcessor.GetBackups(backupPath).Length;
            userRepositoryAcessor.Backup(host, user, password, database, backupPath);
            int currentBackupFileCount = userRepositoryAcessor.GetBackups(backupPath).Length;

            // Assert
            Assert.True(currentBackupFileCount > initialBackupFileCount);
        }
    }
}
