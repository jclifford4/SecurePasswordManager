using System.Security.Cryptography;
using ProgramPrompts;

namespace EncryptionUtility
{
    public class Keygenerator
    {
        public static string GenerateSecureKey()
        {
            try
            {
                using var rng = RandomNumberGenerator.Create();
                byte[] key = new byte[32];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
            catch (SimpleException ex)
            {
                Prompt.HandleException(ex);
                return null;
            }
            catch (Exception ex)
            {
                Prompt.HandleException(ex);
                return null; ;
            }

        }
    }

}
