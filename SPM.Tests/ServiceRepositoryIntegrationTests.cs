using Xunit;
using SPM;
using ServiceRepository;
using Services;
using UserRepository;
using VerifyStringUtility;
namespace SPM.Tests
{
    public class ServiceRepositoryIntegrationTests
    {
        public ServiceRepositoryIntegrationTests()
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
            string fileName = "MySqlBackup_2024-05-30_13-33-13-3313.sql";
            return userRepositoryAcessor.Restore(host, user, password, database, backupPath, fileName);

        }

        [Fact]
        public void CreateService_AddToDatabaseWithUserID_ShouldSucceed()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("Netflix", VerifyStringUtil.CreateGuid(), "testpassword");

            // Act
            bool isAdded = serviceRepository.Add(service, "Initial");
            bool isRestored = RestoreDB();

            // Assert
            Assert.True(isAdded);
            Assert.True(isRestored);

        }

        [Fact]
        public void CreateServiceAndAddToTable_UserDoesNotExist_ShouldFil()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("Netflix", VerifyStringUtil.CreateGuid(), "testpassword");

            // Act
            bool isAdded = serviceRepository.Add(service, "testuser");
            bool isRestored = RestoreDB();

            // Assert
            Assert.False(isAdded);
            Assert.True(isRestored);

        }
    }
}
