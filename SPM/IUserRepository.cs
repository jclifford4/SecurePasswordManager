using UserAccount;

public interface IUserRepository
{
    bool UsernameExists(string username);
    bool Add(User user);
    bool Update(User user, string newUserName);
    bool Delete(User user);
    bool DeleteAll();
}
