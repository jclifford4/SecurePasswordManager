using UserAccount;

public interface IUserRepository
{
    bool UsernameExists(string username);
    void Add(User user);
}
