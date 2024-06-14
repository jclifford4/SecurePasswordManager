using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProgramPrompts;
namespace EncryptionUtility
{
    public class EncryptionUtil
    {
        private static byte[] _encryptionKey;


        static EncryptionUtil()
        {
            string? key = ReadEncryptionCredentials(@"scripts/.my.cnf");

            if (string.IsNullOrEmpty(key))
            {

                throw new SimpleException("Encryption key must be set in the environment variable.\n"
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

        static string ReadEncryptionCredentials(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            string encryption = null;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("encryption="))
                {
                    encryption = trimmedLine.Substring("encryption=".Length).Trim();
                }

                if (encryption != null)
                {
                    // Found both username and password, exit loop
                    break;
                }
            }

            if (encryption == null || string.IsNullOrWhiteSpace(encryption))
                throw new SimpleException("Encryption key is empty or null.");

            return encryption;
        }


    }
}
