using Services;

public interface IServiceRepository
{
    bool ServiceExists(string service);
    bool GuidExists(string guid);
    bool Add(Service service, string username);
    bool Update(Service service, string name);
    bool Delete(Service service);
    bool DeleteAll();
    int Count();
    bool Backup(string host, string user, string password, string database, string backupPath);
    bool Restore(string host, string user, string password, string database, string backupPath, string fileName);
    string[] GetBackups(string backupPath);
}
