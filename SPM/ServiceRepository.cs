using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using Services;
using UserRepository;

namespace ServiceRepository
{

    public class ServiceRepositoryAccessor : IServiceRepository
    {
        private MySqlConnection _connection;

        public ServiceRepositoryAccessor(string user, string password, string dbname)
        {
            string hostname = "localhost";
            string connectionString;
            connectionString = "SERVER=" + hostname + ";" + "DATABASE=" +
            dbname + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";

            // TODO: Create connection string 
            // Server server = new Server(dbname, hostname, user, password);
            // string conString = server.ConnectionString;

            _connection = new MySqlConnection(connectionString);
        }

        public ServiceRepositoryAccessor()
        {
            string? USER = Environment.GetEnvironmentVariable("MYSQL_USER");
            string? PASSWORD = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            const string HOST = "localhost";
            string? DATABASE = Environment.GetEnvironmentVariable("MYSQL_DATABASE_NAME");
            string? BACKUP_PATH = Environment.GetEnvironmentVariable("MYSQL_BACKUP_PATH");

            string connectionString = "SERVER=" + HOST + ";" + "DATABASE=" +
            DATABASE + ";" + "UID=" + USER + ";" + "PASSWORD=" + PASSWORD + ";";

            _connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Add(Service service, int userID)
        {

            if (userID == -1)
                return false;

            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("INSERT INTO services (userID, service, encryptedPassword, guid, creationDate)"
                    + " VALUES (@UserID, @Service, @EncryptedPassword, @Guid, @CreationDate)", _connection);

                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Service", service.Name);
                cmd.Parameters.AddWithValue("@EncryptedPassword", service.EncryptedPassword);
                cmd.Parameters.AddWithValue("@CreationDate", service.CreationDate);
                cmd.Parameters.AddWithValue("@Guid", service.Guid);
                cmd.ExecuteNonQuery();

                _connection.Close();

                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public bool Backup(string host, string user, string password, string database, string backupPath)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("SELECT Count(*) FROM services", _connection);
                int rowCount = Convert.ToInt32(cmd.ExecuteScalar());

                _connection.Close();

                return rowCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

        public bool Delete(Service service, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("DELETE FROM services WHERE service=@Service AND userID=@UserID", _connection);
                cmd.Parameters.AddWithValue("@Service", service.Name);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.ExecuteNonQuery();

                _connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting service " + ex.Message);
                return false;
            }
        }
        public bool DeleteByServiceName(string serviceName, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("DELETE FROM services WHERE service=@Service AND userID=@UserID", _connection);
                cmd.Parameters.AddWithValue("@Service", serviceName);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.ExecuteNonQuery();

                _connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting service " + ex.Message);
                return false;
            }
        }

        public bool DeleteAllByUserID(int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("DELETE FROM services WHERE userID=@UserID", _connection);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.ExecuteNonQuery();

                _connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error cannot delete all services by userID", ex.Message);
                return false;
            }
        }

        public string[] GetBackups(string backupPath)
        {
            throw new NotImplementedException();
        }

        public bool GuidExists(string guid)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("SELECT COUNT(*) FROM services WHERE guid=@Guid", _connection);

                cmd.Parameters.AddWithValue("@Guid", guid);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                _connection.Close();

                return count > 0;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: ", ex.Message);
                return false;
            }
        }

        public bool Restore(string host, string user, string password, string database, string backupPath, string fileName)
        {
            throw new NotImplementedException();
        }

        public bool ServiceExistsByUserID(Service service, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("SELECT COUNT(*) FROM services WHERE userID=@UserID AND service=@Service", _connection);
                cmd.Parameters.AddWithValue("@Service", service.Name);
                cmd.Parameters.AddWithValue("@UserID", userID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                _connection.Close();

                return count > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding service: ", ex.Message);
                return false;
            }
        }
        public bool ServiceExistsByUserID(string serviceName, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("SELECT COUNT(*) FROM services WHERE userID=@UserID AND service=@Service", _connection);
                cmd.Parameters.AddWithValue("@Service", serviceName);
                cmd.Parameters.AddWithValue("@UserID", userID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                _connection.Close();

                return count > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding service: ", ex.Message);
                return false;
            }
        }
        public bool ServiceNameExistsByUserID(string serviceName, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("SELECT COUNT(*) FROM services WHERE userID=@UserID AND service=@Service", _connection);
                cmd.Parameters.AddWithValue("@Service", serviceName);
                cmd.Parameters.AddWithValue("@UserID", userID);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                _connection.Close();

                return count > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding service: ", ex.Message);
                return false;
            }
        }
        public (bool, string) GetEncryptedByUserID(string serviceName, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("SELECT encryptedPassword FROM services WHERE userID=@UserID AND service=@Service", _connection);
                cmd.Parameters.AddWithValue("@Service", serviceName);
                cmd.Parameters.AddWithValue("@UserID", userID);
                var reader = cmd.ExecuteReader();
                string result = string.Empty;
                if (reader.Read())
                {
                    result = reader["encryptedPassword"].ToString();

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        throw new Exception("Error getting encrypted password");
                    }
                }

                reader.Close();
                _connection.Close();

                return (true, result);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding service: ", ex.Message);
                return (false, string.Empty);
            }
        }

        public List<(string, string)> GetAllServicesByUserID(int userID)
        {
            try
            {
                var services = new List<(string, string)>();

                _connection.Open();

                var cmd = new MySqlCommand("SELECT service, encryptedPassword FROM services WHERE userID=@UserID", _connection);
                cmd.Parameters.AddWithValue("@UserID", userID);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string serviceName = reader["service"].ToString();
                    string encrypted = reader["encryptedPassword"].ToString();

                    if (string.IsNullOrWhiteSpace(serviceName) || string.IsNullOrWhiteSpace(encrypted))
                        throw new Exception("Error getting service list");

                    services.Add((serviceName, encrypted));
                }

                return services;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<(string, string)>();
            }
        }

        public bool UpdateServiceEncryption(Service service, int userID)
        {
            try
            {
                _connection.Open();

                var cmd = new MySqlCommand("UPDATE services SET encryptedPassword=@EncryptedPassword, creationDate=@CreationDate " +
                    "WHERE userID=@UserID AND service=@Service", _connection);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Service", service.Name);
                cmd.Parameters.AddWithValue("@EncryptedPassword", service.EncryptedPassword);
                cmd.Parameters.AddWithValue("@CreationDate", service.CreationDate);
                cmd.ExecuteNonQuery();

                _connection.Close();

                return true;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex.Message);
                return false;
            }
        }
    }
}
