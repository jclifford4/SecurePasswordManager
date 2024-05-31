using Xunit;
using SPM;
using EncryptionUtility;
using ServiceRepository;
using Services;
using UserRepository;
using VerifyStringUtility;
using Users;
namespace SPM.Tests
{
    [Collection("SequentialTests")]
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
            var service = new Service("Netflix", "testpassword");
            var userRepositoryAcessor = new UserRepositoryAcessor();


            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName("initial");
            bool isAdded = serviceRepository.Add(service, userID);
            bool isRestored = RestoreDB();

            // Assert
            Assert.NotEqual(-1, userID);
            Assert.True(isAdded);
            Assert.True(isRestored);

        }

        [Fact]
        public void CreateServiceAndAddToTable_UserDoesNotExist_ShouldFail()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("Netflix", "testpassword");
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName("testuser");
            bool isAdded = serviceRepository.Add(service, userID);
            bool isRestored = RestoreDB();

            // Assert
            Assert.Equal(-1, userID);
            Assert.False(isAdded);
            Assert.True(isRestored);

        }

        [Fact]
        public void CreateServiceAndAddToTable_UpdateEncryptedPassword_ShouldSucceed()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("Netflix", "testpassword");

            string initialEncryption = service.EncryptedPassword;
            string updatedEncryption = EncryptionUtil.EncryptString("newtestpassword");
            var updatedService = new Service("Netflix", "newtestpassword");


            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName("Initial");
            bool isAdded = serviceRepository.Add(service, userID);
            bool isUpdated = serviceRepository.UpdateServiceEncryption(updatedService, userID);
            bool isRestored = RestoreDB();

            // Assert
            Assert.NotEqual(initialEncryption, updatedEncryption);
            Assert.True(isAdded);
            Assert.True(isUpdated);
            Assert.True(isRestored);

        }

        [Fact]
        public void CreateServiceAddToTable_DeleteUser_ShouldSucceed()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("Hulu", "hulupassword");
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName("Initial");
            bool isAdded = serviceRepository.Add(service, userID);
            bool isDeleted = serviceRepository.Delete(service, userID);
            bool isRestored = RestoreDB();

            // Assert
            Assert.NotEqual(-1, userID);
            Assert.True(isAdded);
            Assert.True(isDeleted);
            Assert.True(isRestored);
        }



        [Fact]
        public void CreateManyServicesFromSingleUserAddToTable_DeleteAllByUserID_ShouldSucceed()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("Hulu", "hulupassword");
            var service1 = new Service("Netflix", "netflixpassword");
            var service2 = new Service("YouTube", "youtubepassword");
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName("Initial");
            bool isAdded = serviceRepository.Add(service, userID);
            bool isAdded1 = serviceRepository.Add(service1, userID);
            bool isAdded2 = serviceRepository.Add(service2, userID);
            bool isDeleted = serviceRepository.DeleteAllByUserID(userID);
            bool isRestored = RestoreDB();

            // Assert
            Assert.NotEqual(-1, userID);
            Assert.True(isAdded);
            Assert.True(isAdded1);
            Assert.True(isAdded2);
            Assert.True(isDeleted);
            Assert.True(isRestored);
        }

        [Fact]
        public void CheckGuidExists_ShouldFail()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            string guid = "f51d4570-41b7-4442-aa4c-e3be0a2fd68d";

            // Act
            bool guidExists = serviceRepository.GuidExists(guid);

            // Assert
            Assert.False(guidExists);
        }

        [Fact]
        public void AddService_CheckGuidExists_ShouldSucceed()
        {
            // Arrange
            var serviceRepository = new ServiceRepositoryAccessor("testing", "bigpassword", "spmdb");
            var service = new Service("testservice", "testpassword");
            string guid = service.Guid;
            var userRepositoryAcessor = new UserRepositoryAcessor();

            // Act
            int userID = userRepositoryAcessor.GetUserIDByUserName("Initial");
            bool isAdded = serviceRepository.Add(service, userID);
            bool guidExists = serviceRepository.GuidExists(guid);
            bool isRestored = RestoreDB();

            // Assert
            Assert.NotEqual(-1, userID);
            Assert.True(isAdded);
            Assert.True(guidExists);
            Assert.True(isRestored);
        }

    }
}
