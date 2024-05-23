using HashUtility;

namespace UserAccount
{
    public class User
    {
        private string _userName { get; set; }
        private string _passwordHash { get; set; }
        private string? _email { get; set; }
        private string? _dateofbirth { get; set; }
        private List<Tuple<string, string>> _userItemsAndHashes { get; set; }


        public User()
        {
            this._userItemsAndHashes = new List<Tuple<string, string>>();
            this._userName = string.Empty;
            this._passwordHash = string.Empty;
        }


        public User(string UserName, string PasswordHash, string? Email, string? DOB)
        {
            this._userName = UserName;
            this._passwordHash = PasswordHash;
            this._email = Email;
            this._dateofbirth = DOB;
            this._userItemsAndHashes = new List<Tuple<string, string>>();

        }

        public string UserName { get => _userName; }
        public string PasswordHash { get => _passwordHash; }
        public string? Email { get => _email; }
        public string? DOB { get => _dateofbirth; }
        public List<Tuple<string, string>> UserItemsAndHashes { get => _userItemsAndHashes; }

        // Setters
        public void UpdateUserPasswordHashes(Tuple<string, string> newItemHashTuple)
        {
            this._userItemsAndHashes.Add(newItemHashTuple);
        }
        public void UpdateUserName(string newUserName)
        {
            this._userName = newUserName;
        }

        public void UpdateUserEmail(string newUserEmail)
        {
            this._email = newUserEmail;
        }

        public void UpdateUserPasswordHash(string newUserPasswordHash)
        {
            this._passwordHash = newUserPasswordHash;
        }

        public void UpdateUserDateOfBirth(string newUserDateOfBirth)
        {
            this._dateofbirth = newUserDateOfBirth;
        }

        // Getters

        /// <summary>
        /// Gets a requested password hash by item name
        /// </summary>
        /// <param name="itemName">string: requested item</param>
        /// <returns>pair(string, string) or pair(null, null)</returns>
        public (string? item, string? paswwordHash) GetUserPasswordHashByName(string itemName)
        {
            foreach (var pair in this.UserItemsAndHashes)
            {
                if (pair.Item1 == itemName)
                {
                    return (pair.Item1, pair.Item2);
                }

            }
            return (null, null);
        }

        /// <summary>
        /// Display all items the user has saved.
        /// </summary>
        public void ListAllSavedUserItemNames()
        {
            UserItemsAndHashes.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            foreach (var item in this.UserItemsAndHashes)
            {
                Console.WriteLine($"{item.Item1} : {item.Item2}");
            }

        }
        public string? GetUserName()
        {
            return this._userName;
        }

        public string? GetUserPasswordHash()
        {
            return this._passwordHash;
        }

        public string? GetUserEmail()
        {
            return this._email;
        }

        public string? GetUserDateOfBirth()
        {
            return this._dateofbirth;
        }

        // Utility Methods
        public override string ToString()
        {
            return
                $"Username: {GetUserName()}\n" +
                $"Hash: {GetUserPasswordHash()}\n" +
                $"Email: {GetUserEmail()}\n" +
                $"DOB: {GetUserDateOfBirth()}";

        }
    }
}
