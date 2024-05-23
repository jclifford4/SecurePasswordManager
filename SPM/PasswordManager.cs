using System;
using System.Collections.Generic;


class PasswordManager
{
    private Dictionary<string, string> passwordStorage = new Dictionary<string, string>();

    /// <summary>
    /// Encrypt password
    /// </summary>
    /// <param name="storageKey">string: dictionary key</param>
    /// <param name="password">string: provided password</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void StorePassword(string storageKey, string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password), "Password cannot be null");
        }

        string encryptedPassword = EncryptionUtility.EncryptString(password);
        passwordStorage[storageKey] = encryptedPassword;
        Console.WriteLine($"Password stored successfully! {storageKey} : {encryptedPassword}");
    }
    /// <summary>
    /// Decrypt the password
    /// </summary>
    /// <param name="storageKey">string: the key of dictionary</param>
    public void RetrievePassword(string storageKey)
    {
        if (passwordStorage.TryGetValue(storageKey, out var encryptedPassword))
        {
            string decryptedPassword = EncryptionUtility.DecryptString(encryptedPassword);
            Console.WriteLine($"Decrypted Password for {storageKey}: {decryptedPassword}");
        }
        else
        {
            Console.WriteLine("Password not found!");
        }
    }
}
