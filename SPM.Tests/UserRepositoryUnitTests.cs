using Xunit;
using SPM;
using UserRepository;


namespace SPM.Tests
{

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
            Environment.SetEnvironmentVariable("BACKUP_PATH", "J:");
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
            string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");


            // Act
            userRepositoryAcessor.Backup(host, user, password, database, Path.Combine(backupPath, $"backup_{dateTime}.sql"));

            // Assert
            Assert.False(host == null);
            Assert.False(user == null);
            Assert.False(password == null);
            Assert.False(backupPath == null);
            Assert.True(File.Exists(backupPath));

        }

        [Fact]
        public void CanOpenConnection_ReturnSuccess()
        {
            var userRepositoryAcessor = new UserRepositoryAcessor();

            bool isConnected = userRepositoryAcessor.OpenDatabaseConnection();

            Assert.True(isConnected);

        }

        [Fact]
        public void CanCloseConnection_ReturnSuccess()
        {
            var userRepositoryAcessor = new UserRepositoryAcessor();
            bool isClosed = userRepositoryAcessor.CloseDatabaseConnection();

            Assert.True(isClosed);

        }
    }
}
