using HashUtility;
using UserUtility;

namespace UserAccount
{
    public class User
    {
        private string _userName { get; set; }
        private string _passwordHash { get; set; }
        private string _creationDate { get; set; }
        // private List<Tuple<string, string>> _userItemsAndHashes { get; set; }

        public string UserName { get => _userName; }
        public string PasswordHash { get => _passwordHash; }
        public string CreationDate { get => _creationDate; }
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

        public User(string UserName, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(UserName))
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(UserName));

            if (string.IsNullOrWhiteSpace(PasswordHash))
                throw new ArgumentException("Username cannot be empty or whitespace.", nameof(UserName));

            this._userName = UserName;
            this._passwordHash = UserUtil.HashPassword(UserName, providedPassword);
            this._creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
