namespace DatabaseUtility
{
    public static class DatabaseUtil
    {

        private static readonly HashSet<char> illegalCharactersSet =
                        ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
                         '=', '+', '\\', '{', '}', '|', ',', '.', '\"',
                         '<', '>', '/', '?', '~', '\'', ';', ':'];

        internal static bool IsValidString(string value)
        {
            if (value.Length < 3 || value.Length > 32)
                return false;

            foreach (var character in value)
            {
                if (illegalCharactersSet.Contains(character))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Check password for minimum length and illegal characters.
        /// </summary>
        /// <param name="providedPassword">String</param>
        /// <returns>true or false</returns>
        internal static bool IsValidPassword(string providedPassword)
        {
            if (providedPassword.Length < 8 || providedPassword.Length > 128)
                return false;

            return true;
        }

        // TODO: hash password
        internal static string HashDataBasePassword(string password)
        {
            throw new NotImplementedException();
        }
    }
}
