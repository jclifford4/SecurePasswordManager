using HashUtility;
using UserUtility;

namespace Users
{
    public class User
    {
        private string _userName { get; set; }
        private string _passwordHash { get; set; }
        private string _creationDate { get; set; }
        private string _guid { get; set; }

        public string UserName { get => _userName; }
        public string PasswordHash { get => _passwordHash; }
        public string CreationDate { get => _creationDate; }
        public string Guid { get => _guid; }

        public User(string userName, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(userName));

            if (!UserUtil.IsValidUsername(userName))
                throw new ArgumentException("Username has illegal characters.", nameof(userName));

            if (string.IsNullOrWhiteSpace(providedPassword))
                throw new ArgumentException("Password cannot be empty or whitespace.");

            if (!UserUtil.IsValidPassword(providedPassword))
                throw new ArgumentException("Password has illegal characters or < 8 chars or > 128 chars.");


            this._userName = userName;
            this._passwordHash = UserUtil.HashPassword(userName, providedPassword);
            this._creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this._guid = UserUtil.GenerateGuidAsString();

        }

        public User(string userName, string providedPassword, string guid)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(userName));

            if (!UserUtil.IsValidUsername(userName))
                throw new ArgumentException("Username has illegal characters.", nameof(userName));

            if (string.IsNullOrWhiteSpace(guid))
                throw new ArgumentException("Guid cannot be empty or whitespace.", nameof(guid));

            if (!UserUtil.IsValidGuid(guid))
                throw new ArgumentException("Guid has illegal characters.", nameof(guid));

            if (string.IsNullOrWhiteSpace(providedPassword))
                throw new ArgumentException("Password cannot be empty or whitespace.");

            if (!UserUtil.IsValidPassword(providedPassword))
                throw new ArgumentException("Password has illegal characters or < 8 chars or > 128 chars.");


            this._userName = userName;
            this._passwordHash = UserUtil.HashPassword(userName, providedPassword);
            this._creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this._guid = guid;
        }
    }
}
