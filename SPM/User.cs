using HashUtility;
using UserUtility;

namespace UserAccount
{
    public class User
    {
        private string _userName { get; set; }
        private string _passwordHash { get; set; }
        private string _creationDate { get; set; }
        private string _guid { get; set; }
        // private List<Tuple<string, string>> _userItemsAndHashes { get; set; }

        public string UserName { get => _userName; }
        public string PasswordHash { get => _passwordHash; }
        public string CreationDate { get => _creationDate; }
        public string Guid { get => _guid; }
        // public List<Tuple<string, string>> UserItemsAndHashes { get => _userItemsAndHashes; }

        // public User()
        // {
        //     // this._userItemsAndHashes = new List<Tuple<string, string>>();
        //     this._userName = string.Empty;
        //     this._passwordHash = string.Empty;
        //     this._creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // }

        // public User(string UserName)
        // {
        //     if (string.IsNullOrWhiteSpace(UserName))
        //         throw new ArgumentException("Username cannot be empty or whitespace.", nameof(UserName));

        //     this._userName = UserName;
        //     this._passwordHash = string.Empty;
        //     this._creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // }

        // public User(string userName)
        // {
        //     this._userName = userName;
        //     this._passwordHash = string.Empty;
        // }

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
            // this._userItemsAndHashes = new List<Tuple<string, string>>();

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
            // this._userItemsAndHashes = new List<Tuple<string, string>>();

        }

        // Setters
        // public void UpdateUserPasswordHashes(Tuple<string, string> newItemHashTuple)
        // {
        //     this._userItemsAndHashes.Add(newItemHashTuple);
        // }
        // public void UpdateUserName(string newUserName)
        // {
        //     this._userName = newUserName;
        // }


        // public void UpdateUserPasswordHash(string newUserPasswordHash)
        // {
        //     this._passwordHash = newUserPasswordHash;
        // }



        // // Getters

        // /// <summary>
        // /// Gets a requested password hash by item name
        // /// </summary>
        // /// <param name="itemName">string: requested item</param>
        // /// <returns>pair(string, string) or pair(null, null)</returns>
        // public (string? item, string? paswwordHash) GetUserPasswordHashByName(string itemName)
        // {
        //     foreach (var pair in this.UserItemsAndHashes)
        //     {
        //         if (pair.Item1 == itemName)
        //         {
        //             return (pair.Item1, pair.Item2);
        //         }

        //     }
        //     return (null, null);
        // }

        // /// <summary>
        // /// Display all items the user has saved.
        // /// </summary>
        // public void ListAllSavedUserItemNames()
        // {
        //     UserItemsAndHashes.Sort((x, y) => x.Item1.CompareTo(y.Item1));

        //     foreach (var item in this.UserItemsAndHashes)
        //     {
        //         Console.WriteLine($"{item.Item1} : {item.Item2}");
        //     }

        // }
        // public string? GetUserName()
        // {
        //     return this._userName;
        // }

        // public string? GetUserPasswordHash()
        // {
        //     return this._passwordHash;
        // }



        // // Utility Methods
        // public override string ToString()
        // {
        //     return
        //         $"Username: {GetUserName()}\n" +
        //         $"Hash: {GetUserPasswordHash()}\n";
        // }
    }
}
