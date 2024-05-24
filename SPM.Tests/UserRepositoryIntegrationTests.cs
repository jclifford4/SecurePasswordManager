using Xunit;
using UserRepository;

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
    }
}
