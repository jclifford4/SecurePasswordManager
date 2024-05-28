using Users;

public interface IUserRepository
{
    bool UsernameExists(string username);
    bool GuidExists(string guid);
    bool Add(User user);
    bool Update(User user, string newUserName);
    bool Delete(User user);
    bool DeleteAll();
    int Count();
    bool Backup(string host, string user, string password, string database, string backupPath);
    bool Restore(string host, string user, string password, string database, string backupPath, string fileName);
    string[] GetBackups(string backupPath);
}
