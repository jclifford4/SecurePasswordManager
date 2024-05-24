using UserAccount;

public interface IUserRepository
{
    bool UsernameExists(string username);
    bool Add(User user);
}
