using Services;

public interface IServiceRepository
{
    bool ServiceExistsByUserID(Service service, int userID);
    bool GuidExists(string guid);
    bool Add(Service service, int userID);
    bool UpdateServiceEncryption(Service service, int userID);
    bool Delete(Service service, int userID);
    bool DeleteAllByUserID(int userID);
    int Count();
    bool Backup(string host, string user, string password, string database, string backupPath);
    bool Restore(string host, string user, string password, string database, string backupPath, string fileName);
    string[] GetBackups(string backupPath);
}
