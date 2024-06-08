using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace EncryptionUtility
{
    public class EncryptionUtil
    {
        private static byte[] _encryptionKey;


        static EncryptionUtil()
        {
            string? key = Environment.GetEnvironmentVariable("MYSQL_ENCRYPTION_KEY");
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Encryption key must be set in the environment variable.\n"
                                            + "$env:MYSQL_ENCRYPTION_KEY = \"your-32byte-base64-key\"");
            }

            byte[] keyBytes = Convert.FromBase64String(key);
            if (keyBytes.Length != 32)
            {
                throw new ArgumentException("Encryption key must be a 32-byte string.");
            }
            _encryptionKey = keyBytes;
        }

        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentException("String to encrypt cannot be null or empty.");
            }

            if (plainText.Length < 8 || plainText.Length > 128)
            {
                throw new ArgumentException("String length must be 8 - 128.");
            }

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {

                aes.Key = _encryptionKey;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }


        public static string GenerateSecureKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] key = new byte[32];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }

        }


    }
}
