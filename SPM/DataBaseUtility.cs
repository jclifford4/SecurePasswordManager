using System.Linq.Expressions;
using MySql.Data.MySqlClient;
namespace DataBaseUtility
{
    /// <summary>
    /// Encapsulate private class 'PrivateDatabaseManager.
    /// </summary>
    public class DatabaseManagerAcessor
    {
        private readonly PrivateDatabaseManager _privateDatabaseManager;

        public DatabaseManagerAcessor()
        {
            _privateDatabaseManager = new PrivateDatabaseManager();
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
        /// <summary>
        /// Private Database Manager class
        /// </summary>
        class PrivateDatabaseManager
        {
            private MySqlConnection connection;
            private string server;
            private string database;
            private string uid;
            private string password;

            //Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            public PrivateDatabaseManager()
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
                server = "localhost";
                database = "spmdb";
                uid = "root";
                password = "FullstackDev12!";
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

                connection = new MySqlConnection(connectionString);
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
                    connection.Open();
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
                    connection.Close();
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
                    MySqlCommand cmd = new MySqlCommand(query, connection);

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
                    cmd.Connection = connection;

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
                    MySqlCommand cmd = new MySqlCommand(query, connection);
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
                    MySqlCommand cmd = new MySqlCommand(query, connection);

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
