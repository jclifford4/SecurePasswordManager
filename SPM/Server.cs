using UserRepositoryUtility;
namespace DBServer
{
    public class Server
    {
        private string _database_name { get; set; }
        private string _hostname { get; set; }
        private string _user { get; set; }
        private string _password_hash { get; set; }
        private string _connection_string { get; set; }


        public Server(string name, string user, string passwordhash)
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Database name cannot be empty or whitespace.", nameof(name));

            if (!UserRepositoryUtil.IsValidString(name))
                throw new ArgumentException("Database name has illegal characters.", nameof(name));

            // if (string.IsNullOrWhiteSpace(hostname))
            //     throw new ArgumentException("Hostname cannot be empty or whitespace.", nameof(hostname));

            // if (!DatabaseUtil.IsValidString(hostname))
            //     throw new ArgumentException("Hostname has illegal characters.", nameof(hostname));

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(user));

            if (!UserRepositoryUtil.IsValidString(user))
                throw new ArgumentException("Username has illegal characters.", nameof(user));

            if (string.IsNullOrWhiteSpace(passwordhash))
                throw new ArgumentException("Password cannot be empty or whitespace.");

            if (!UserRepositoryUtil.IsValidPassword(passwordhash))
                throw new ArgumentException("Password has illegal characters.");

            this._database_name = name;
            this._hostname = "localhost";
            this._user = user;
            this._password_hash = passwordhash;

            this._connection_string = CreateConnectionString(name, _hostname, user, passwordhash);
        }

        public static string CreateConnectionString(string name, string hostname, string user, string password)
        {
            return "SERVER=" + hostname + ";" + "DATABASE=" +
                name + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
        }

        public string DatabaseName { get => _database_name; }
        public string Hostname { get => _hostname; }
        public string User { get => _user; }
        public string PasswordHash { get => _password_hash; }
        public string ConnectionString { get => _connection_string; }
    }
}
