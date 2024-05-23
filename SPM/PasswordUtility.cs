
namespace PasswordUtility
{
    public static class PasswordUtil
    {
        public static string GenerateNewPassword(int length)
        {
            return GeneratePassword(length);
        }
        private static string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghi" +
                                "jklmnopqrstuvwxyz0123456789!?_()-+*";
            var random = new Random();
            var passwordChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(passwordChars);
        }
    }

}
