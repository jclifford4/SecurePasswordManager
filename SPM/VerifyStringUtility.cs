using Microsoft.AspNetCore.Identity;
namespace VerifyStringUtility
{
    public static class VerifyStringUtil
    {
        private static readonly HashSet<char> illegalCharactersSet =
                        ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
                         '=', '+', '\\', '{', '}', '|', ',', '.', '\"',
                         '<', '>', '/', '?', '~', '\'', ';', ':'];
        public static string CreateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool isValidUsername(string username)
        {
            if (username.Length < 3 || username.Length > 32)
                return false;

            foreach (var character in username)
            {
                if (illegalCharactersSet.Contains(character))
                {
                    return false;
                }
            }

            return true;
        }
        public static bool isValidPassword(string password)
        {
            if (password.Length < 8 || password.Length > 128)
                return false;

            return true;
        }
        public static bool isValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }
    }
}
