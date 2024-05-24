using Xunit;
using SPM;
using UserRepository;



public class UserRepositoryUnitTests
{
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
