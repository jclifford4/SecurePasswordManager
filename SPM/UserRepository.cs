using System.Diagnostics;
using MySql.Data.MySqlClient;
using UserAccount;
namespace UserRepository
{
    /// <summary>
    /// Encapsulate private class 'PrivateDatabaseManager.
    /// </summary>
    public class UserRepositoryAcessor : IUserRepository
    {
        private readonly PrivateRepositoryAccesor _privateDatabaseManager;

        public UserRepositoryAcessor()
        {
            _privateDatabaseManager = new PrivateRepositoryAccesor();
        }

        /// <summary>
        /// Public facing database connection
        /// </summary>
        /// <returns>true or false</returns>
        public bool OpenDatabaseConnection()
        {
            return _privateDatabaseManager.Connect();
        }

        /// <summary>
        /// Public facing method to close database connection
        /// </summary>
        /// <returns>true or false</returns>
        public bool CloseDatabaseConnection()
        {
            return _privateDatabaseManager.Close();
        }

        /// <summary>
        /// Select statement from users table
        /// </summary>
        /// <returns>list[list[UserID], list[UserName], list[PasswordHash]]</returns>
        public List<string>[] Select()
        {
            return _privateDatabaseManager.Select();
        }

        /// <summary>
        /// Insert user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        public void Insert(string username, string passwordHash, string datetime)
        {
            _privateDatabaseManager.Insert(username, passwordHash, datetime);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="username"></param>
        // public void Delete(string username)
        // {
        //     _privateDatabaseManager.Delete(username);
        // }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="passwordhash">string</param>
        /// <param name="datetime">DateTime</param>
        public void Update(string oldusername, string newusername, string passwordhash, string datetime)
        {
            _privateDatabaseManager.Update(oldusername, newusername, passwordhash, datetime);
        }

        public bool UsernameExists(string username)
        {
            return _privateDatabaseManager.UsernameExists(username);
        }


        public bool Add(User user)
        {
            return _privateDatabaseManager.Add(user);
        }

        public bool Update(User user, string newUserName)
        {
            return _privateDatabaseManager.Update(user, newUserName);
        }

        public bool Delete(User user)
        {
            return _privateDatabaseManager.Delete(user);
        }

        public bool DeleteAll()
        {
            return _privateDatabaseManager.DeleteAll();
        }

        public int Count()
        {
            return _privateDatabaseManager.Count();
        }

        public bool Backup(string host, string user, string password, string database, string backupPath)
        {
            return _privateDatabaseManager.Backup(host, user, password, database, backupPath);
        }
        public bool Restore(string host, string user, string password, string database, string backupPath, string fileName)
        {
            return _privateDatabaseManager.Restore(host, user, password, database, backupPath, fileName);
        }

        public string[] GetBackups(string backupPath)
        {
            return _privateDatabaseManager.GetBackups(backupPath);
        }

        /// <summary>
        /// Private Database Manager class
        /// </summary>
        class PrivateRepositoryAccesor
        {
            private MySqlConnection _connection;
            private string _server;
            private string _database;
            private string _uid;
            private string _password;

            //Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            public PrivateRepositoryAccesor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            {
                Initialize();
            }

            /// <summary>
            /// Initialize server connection string
            /// </summary>
            private void Initialize()
            {
                _server = "localhost";
                _database = "spmdb";
                _uid = "root";
                _password = "FullstackDev12!";
                string connectionString;
                connectionString = "SERVER=" + _server + ";" + "DATABASE=" +
                _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";";

                _connection = new MySqlConnection(connectionString);
            }

            /// <summary>
            /// Encapsulate database connection
            /// </summary>
            /// <returns>true or false</returns>
            public bool Connect()
            {
                return OpenConnection();
            }
            /// <summary>
            /// Encapsulate closing databaase connection
            /// </summary>
            /// <returns>true or false</returns>
            public bool Close()
            {
                return CloseConnection();
            }

            /// <summary>
            /// Open a connection to the database.
            /// </summary>
            /// <returns>true or false</returns>
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

            /// <summary>
            /// Close database connection
            /// </summary>
            /// <returns>true or false</returns>
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

            /// <summary>
            /// Insert new user into database
            /// </summary>
            /// <param name="username">string</param>
            /// <param name="passwordhash">string</param>
            public void Insert(string username, string passwordhash, string datetime)
            {

                string query = $"INSERT INTO users (userName, passwordHash, creationDate) Values('{username}', '{passwordhash}', '{datetime}')";

                if (this.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, _connection);

                    cmd.ExecuteReader();


                    this.CloseConnection();
                }
            }

            /// <summary>
            /// Update user statement
            /// </summary>
            /// <param name="username">string</param>
            /// <param name="passwordhash">string</param>
            /// <param name="datetime">DateTime</param>
            public void Update(string oldusername, string newusername, string passwordhash, string datetime)
            {
                string query = $"UPDATE users SET userName='{newusername}', passwordHash='{passwordhash}', creationDate='{datetime}' WHERE userName='{oldusername}'";

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = _connection;

                    cmd.ExecuteReader();

                    this.CloseConnection();
                }
            }


            /// <summary>
            /// Delete User from database
            /// </summary>
            /// <param name="username">string</param>
            // public void Delete(string username)
            // {
            //     string query = $"DELETE FROM users WHERE userName= '{username}'";

            //     if (this.OpenConnection() == true)
            //     {
            //         MySqlCommand cmd = new MySqlCommand(query, _connection);
            //         cmd.ExecuteReader();
            //         this.CloseConnection();
            //     }

            // }


            /// <summary>
            /// Request a select statement to database
            /// </summary>
            /// <returns>List<string></returns>
            public List<string>[] Select()
            {
                string query = "SELECT * FROM users";

                List<string>[] list = new List<string>[3];
                list[0] = new List<string>();   // userid
                list[1] = new List<string>();   // usernames
                list[2] = new List<string>();   // passwordhashes


                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, _connection);

                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        list[0].Add(dataReader["userID"] + "");
                        list[1].Add(dataReader["userName"] + "");
                        list[2].Add(dataReader["passwordHash"] + "");
                    }

                    dataReader.Close();

                    this.CloseConnection();

                    return list;
                }
                else
                {
                    return list;
                }
            }
            /// <summary>
            /// Adds user to DB
            /// </summary>
            /// <param name="user">User</param>
            internal bool Add(User user)
            {

                try
                {
                    _connection.Open();

                    var cmd = new MySqlCommand("INSERT INTO users (userName, passwordHash, creationDate)"
                        + " VALUES (@UserName, @PasswordHash, @CreationDate)", _connection);

                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@CreationDate", user.CreationDate);
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
            /// <summary>
            /// Checks if username is in DB
            /// </summary>
            /// <param name="username">string</param>
            /// <returns>true or false</returns>
            internal bool UsernameExists(string username)
            {
                try
                {
                    _connection.Open();

                    var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE userName = @UserName", _connection);
                    cmd.Parameters.AddWithValue("@UserName", username);
                    var result = Convert.ToInt32(cmd.ExecuteScalar());

                    _connection.Close();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error checking usersnames" + ex.Message);
                    return true;
                }

            }

            internal bool Update(User user, string newUserName)
            {


                try
                {
                    _connection.Open();

                    var cmd = new MySqlCommand
                        ("UPDATE users SET userName=@NewUserName, passwordHash=@PasswordHash,"
                          + " creationDate=@CreationDate WHERE userName=@UserName");
                    cmd.Parameters.AddWithValue("@PasswordHash", user.UserName);
                    cmd.Parameters.AddWithValue("@NewUserName", newUserName);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@CreationDate", user.CreationDate);
                    cmd.ExecuteNonQuery();

                    _connection.Close();

                    return true;

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("There was an error updating the user " + ex.Message);
                    return false;
                }
            }

            internal bool Delete(User user)
            {
                try
                {
                    _connection.Open();

                    var cmd = new MySqlCommand("DELETE FROM users WHERE userName=@UserName", _connection);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.ExecuteNonQuery();

                    _connection.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deleting user " + ex.Message);
                    return false;
                }
            }

            internal bool DeleteAll()
            {
                try
                {
                    _connection.Open();

                    var cmd = new MySqlCommand("DELETE FROM users", _connection);
                    cmd.ExecuteNonQuery();

                    _connection.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }

            internal int Count()
            {
                try
                {
                    _connection.Open();

                    var cmd = new MySqlCommand("SELECT Count(*) FROM users", _connection);
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


            //Backup
            internal bool Backup(string host, string user, string password, string database, string backupPath)
            {
                try
                {
                    // DateTime Time = DateTime.Now;
                    // int year = Time.Year;
                    // int month = Time.Month;
                    // int day = Time.Day;
                    // int hour = Time.Hour;
                    // int minute = Time.Minute;
                    // int second = Time.Second;
                    // int millisecond = Time.Millisecond;
                    string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ms");


                    string path = backupPath + "MySqlBackup"
                    + dateTime + ".sql";
                    StreamWriter file = new StreamWriter(path);

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysqldump";
                    psi.RedirectStandardInput = false;
                    psi.RedirectStandardOutput = true;
                    psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                        user, password, host, database);
                    psi.UseShellExecute = false;

                    Process process = Process.Start(psi);

                    string output;
                    output = process.StandardOutput.ReadToEnd();
                    file.WriteLine(output);
                    process.WaitForExit();
                    file.Close();
                    process.Close();

                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }

            //Restore
            public bool Restore(string host, string user, string password, string database, string backupPath, string fileName)
            {
                try
                {
                    string path = backupPath + fileName;
                    StreamReader file = new StreamReader(path);
                    string input = file.ReadToEnd();
                    file.Close();

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysql";
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardOutput = false;
                    psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                        user, password, host, database);
                    psi.UseShellExecute = false;

                    Process process = Process.Start(psi);

                    process.StandardInput.WriteLine(input);
                    process.StandardInput.Close();
                    process.WaitForExit();
                    process.Close();


                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }

            internal string[] GetBackups(string backupPath)
            {
                string directoryPath = backupPath;

                // Check if the directory exists
                if (Directory.Exists(directoryPath))
                {
                    // Get all files in the directory and its subdirectories
                    string[] fileNames = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                    return fileNames;
                }
                else
                {
                    Console.WriteLine("The directory does not exist.");
                    return [];
                }
            }
        }
    }
}
