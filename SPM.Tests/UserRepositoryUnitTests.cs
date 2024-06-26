using Xunit;
using SPM;
using UserRepository;


namespace SPM.Tests
{

    [Collection("SequentialTests")]
    public class UserRepositoryUnitTests
    {

        public UserRepositoryUnitTests()
        {
            // Set environment variables for the test run
            Environment.SetEnvironmentVariable("ENCRYPTION_KEY", "+BMnIcKdA/q8ie2jlP3sCmjni9dEAWiYGZyn7gNPa3A=");
            Environment.SetEnvironmentVariable("HOST", "localhost");
            Environment.SetEnvironmentVariable("USER", "testing");
            Environment.SetEnvironmentVariable("PASSWORD", "bigpassword");
            Environment.SetEnvironmentVariable("DATABASE", "spmdb");
            Environment.SetEnvironmentVariable("BACKUP_PATH", "J:\\dotnetProjects\\SecurePasswordManager\\SPM\\SPMDatabase\\backups\\");
        }





        [Fact]
        public void BackupDatabase_ReturnSuccess()
        {

            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            string host = Environment.GetEnvironmentVariable("HOST");
            string user = Environment.GetEnvironmentVariable("USER");
            string password = Environment.GetEnvironmentVariable("PASSWORD");
            string database = Environment.GetEnvironmentVariable("DATABASE");
            string backupPath = Environment.GetEnvironmentVariable("BACKUP_PATH");


            // Act
            bool isBackedUp = userRepositoryAcessor.Backup(host, user, password, database, backupPath);

            // Assert
            Assert.False(host == null);
            Assert.False(user == null);
            Assert.False(password == null);
            Assert.False(backupPath == null);
            Assert.True(isBackedUp);

        }
        [Fact]
        public void RestoreDatabase_ReturnSuccess()
        {

            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            string host = Environment.GetEnvironmentVariable("HOST");
            string user = Environment.GetEnvironmentVariable("USER");
            string password = Environment.GetEnvironmentVariable("PASSWORD");
            string database = Environment.GetEnvironmentVariable("DATABASE");
            string backupPath = Environment.GetEnvironmentVariable("BACKUP_PATH");
            string fileName = "MySqlBackup_2024-05-30_13-33-13-3313.sql";


            // Act
            bool isRestored = userRepositoryAcessor.Restore(host, user, password, database, backupPath, fileName);

            // Assert
            Assert.False(host == null);
            Assert.False(user == null);
            Assert.False(password == null);
            Assert.False(backupPath == null);
            Assert.True(isRestored);

        }

        [Fact]
        public void CanOpenConnection_ReturnSuccess()
        {

            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            bool isConnected = userRepositoryAcessor.OpenDatabaseConnection();

            // Assert
            Assert.True(isConnected);

        }

        [Fact]
        public void CanCloseConnection_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            bool isClosed = userRepositoryAcessor.CloseDatabaseConnection();

            // Assert
            Assert.True(isClosed);

        }

        [Fact]
        public void CanGetMySqlBackupFiles_ReturnSuccess()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            string backupPath = Environment.GetEnvironmentVariable("BACKUP_PATH");

            // Act
            string[] fileNames = userRepositoryAcessor.GetBackups(backupPath);

            // Assert
            Assert.True(fileNames.Length > 0);
        }

        [Fact]
        public void CanGetUserIDByUsername_UsernameExists_ShouldSucceed()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            string username = "Initial";

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName(username);

            // Assert
            Assert.NotEqual(-1, userID);
        }

        [Fact]
        public void CanGetUserIDByUsername_UsernameDoesNotExists_ShouldFail()
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();
            string username = "bob";

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName(username);

            // Assert
            Assert.Equal(-1, userID);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CanGetUserIDByUsername_WithNullOrEmptyUsername_ShouldFail(string username)
        {
            // Arrange
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName(username);

            // Assert
            Assert.Equal(-1, userID);


        }
    }
}
