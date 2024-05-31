using Services;

public interface IServiceRepository
{
    bool ServiceExists(string service, string username);
    bool GuidExists(string guid);
    bool Add(Service service, int userID);
    bool UpdateServiceEncryption(Service service, int userID);
    bool Delete(Service service, string username);
    bool DeleteAll();
    int Count();
    bool Backup(string host, string user, string password, string database, string backupPath);
    bool Restore(string host, string user, string password, string database, string backupPath, string fileName);
    string[] GetBackups(string backupPath);
}
