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
        public void Delete(string username)
        {
            _privateDatabaseManager.Delete(username);
        }

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

        //TODO: Implement
        public bool UsernameExists(string username)
        {
            return _privateDatabaseManager.UsernameExists(username);
        }


        public bool Add(User user)
        {
            return _privateDatabaseManager.Add(user);
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
            public void Delete(string username)
            {
                string query = $"DELETE FROM users WHERE userName= '{username}'";

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, _connection);
                    cmd.ExecuteReader();
                    this.CloseConnection();
                }

            }


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
                _connection.Open();

                var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE userName = @UserName", _connection);
                cmd.Parameters.AddWithValue("@UserName", username);
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;

            }

            // //Count statement
            // public int Count()
            // {
            // }

            // //Backup
            // public void Backup()
            // {
            // }

            // //Restore
            // public void Restore()
            // {
            // }
        }
    }
}
