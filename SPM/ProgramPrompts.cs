using System.Dynamic;
using EncryptionUtility;
using HashUtility;
using Microsoft.AspNetCore.Identity;
using Mysqlx.Session;
using ServiceRepository;
using Services;
using UserRepository;
using Users;
using UserUtility;
namespace ProgramPrompts

{
    public class Prompt
    {

        public Prompt() { }

        // ANSI escape code constants
        public const string RESET = "\u001b[0m";
        public const string RED = "\u001b[31m";
        public const string GREEN = "\u001b[32m";
        public const string YELLOW = "\u001b[33m";
        public const string BLUE = "\u001b[34m";
        public const string PURPLE = "\u001b[35m";
        public const string CYAN = "\u001b[36m";

        /*
            User Prompts
        */
        public static void StartUp()
        {
            string file = "art/logoansi.ans";
            try
            {
                Console.CursorVisible = false;
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Console.WriteLine(line);
                        TypeMessageWithTime(line, 10);
                    }
                    Console.WriteLine();
                    Console.CursorVisible = true;

                }
            }
            catch (Exception ex)
            {
                Console.CursorVisible = true;
                HandleException(ex);
            }
            Console.WriteLine(YELLOW + "  [-----Secure Password Manager-----]" + RESET);
        }

        public static void Help()
        {
            Console.WriteLine(YELLOW + "\t  [h -help q -quit]" + RESET);
        }

        public static void PromptForUsername()
        {
            Console.Write(GREEN + "Username: " + RESET);
        }


        /*
            Service Prompts
        */

        /*
            Databse Prompts
        */

        /*
            General Prompts
        */

        public static void SPMIndicator()
        {
            Console.Write(BLUE + "[SPM" + RED + "~" + BLUE + "] " + RESET);
        }
        public static void PromptForPassword()
        {
            Console.Write(GREEN + "Password: " + RESET);
        }

        public static void PromptForMasterPassword()
        {

            Console.Write(GREEN + "Master Password: " + RESET);
        }

        public static void PromptForRepeatPassword()
        {

            Console.Write(GREEN + "Retype Password: " + RESET);
        }

        public static void ClearConsole()
        {
            Console.Clear();
        }

        public static string? GetSensitiveConsoleText()
        {
            try
            {
                string? input = Console.ReadLine();
                if (isValidInput(input))
                    return input;

                throw new SimpleException("Invalid Input");
            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return "error";
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return "error";
            }
        }
        public static string? GetSensitiveUsernameConsoleText()
        {
            try
            {
                string? input = Console.ReadLine();

                if (isValidUsername(input))
                    return input;

                throw new SimpleException("Username is invalid");

            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return "error";
            }
            catch (Exception ex)
            {
                HandleException(ex);
                // Console.WriteLine(RED + ex.Message + RESET);
                return "error";
            }
        }
        public static bool GetSensitivePasswordConsoleText(string? password)
        {
            try
            {
                // string? input = Console.ReadLine();
                if (isValidPassord(password))
                    return true;

                throw new SimpleException("Password is invalid");
            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }
        public static string? GetNonSensitiveConsoleText()
        {
            try
            {
                string? input = Console.ReadLine();
                if (isValidSPMNonSensitiveInput(input))
                    return input;
                else
                {
                    throw new SimpleException($"Invalid Input: length({input.Length}) -> [Min: 0, Max: 16]");
                }
            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return "error";
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return "error";
            }
        }

        public string CreateNewUsername()
        {
            var userRepoAcess = new UserRepositoryAcessor();

            PromptForUsername();
            string username = GetSensitiveUsernameConsoleText();
            if (username.Equals("error") || string.IsNullOrWhiteSpace(username))
                return "error";

            bool userExists = userRepoAcess.UsernameExists(username);

            if (userExists)
                return "error";

            return username;
        }

        public string CreateNewMasterPassword()
        {
            PromptForPassword();
            string masterPassword = HashUtil.ReadPassword();
            Console.WriteLine();
            PromptForRepeatPassword();
            string duplicateMasterPassword = HashUtil.ReadPassword();
            Console.WriteLine();
            if (duplicateMasterPassword.Equals(masterPassword))
            {
                return masterPassword;
            }

            return "error";

        }

        public bool CreateNewUserProfile()
        {
            var userRepoAcess = new UserRepositoryAcessor();
            Console.WriteLine(YELLOW + "User Profile Creation: " + BLUE + "\n\tThis password is not saved. " +
                                    "\n\tRemember it otherwise you will lose access to all other " +
                                    "passwords for this user." +
                                    "\n\tA new master password can be made only if you provide the old one." + RESET);

            try
            {
                string username = CreateNewUsername();
                if (username.Equals("error"))
                    throw new SimpleException("Username Error: may have illegal characters");

                string password = CreateNewMasterPassword();
                if (password.Equals("error"))
                    throw new SimpleException("Password Error");


                User user = new User(username, password);
                return userRepoAcess.Add(user);
            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }


        public bool ProgramOptions(string input)
        {

            switch (input)
            {
                case "":
                    break;
                case "h":
                    ClearConsole();
                    ShowHelpCommands();
                    break;

                case "cr":
                    ClearConsole();
                    if (CreateNewUserProfile())
                        Console.WriteLine(YELLOW + "Profile Created" + RESET);
                    break;

                case "cl":
                    ClearConsole();
                    break;

                case "lu":
                    ClearConsole();
                    var userRepositoryAcessor = new UserRepositoryAcessor();
                    var usersList = userRepositoryAcessor.GetAllUsersAsListOfStrings();

                    Console.WriteLine(YELLOW + "-----User List-----" + RESET);
                    foreach (var user in usersList)
                    {
                        Console.WriteLine(BLUE + user + RESET);
                    }
                    Console.WriteLine(YELLOW + "-------------------" + RESET);
                    Console.WriteLine();
                    break;

                case "lg":
                    ClearConsole();
                    var (isLoggedIn, username) = Login();
                    if (isLoggedIn)
                    {

                        Console.CursorVisible = false;
                        string[] spinner = { "/", "-", "\\", "|" }; // Characters for spinning animation
                        for (int i = 0; i < 20; i++)
                        {
                            if (i < 8)
                            {
                                Console.Write(RED + spinner[i % spinner.Length] + RESET);
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
                            }
                            if (i > 8 && i < 14)
                            {
                                Console.Write(YELLOW + spinner[i % spinner.Length] + RESET);
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
                            }
                            else
                            {
                                Console.Write(GREEN + spinner[i % spinner.Length] + RESET);
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
                            }

                            Thread.Sleep(100); // Adjust sleep time for speed of animation

                        }
                        Console.CursorVisible = true;
                        // Console.WriteLine("\n\t" + WelcomeMessage(username) + "\n");
                        TypeMessage(WelcomeMessage(username) + "\n");
                        // ClearConsole();

                        while (isLoggedIn)
                        {
                            SPMUserIndicator(username);
                            Console.Write(CYAN);
                            isLoggedIn = LoginOptions(username, GetNonSensitiveConsoleText());


                            if (!isLoggedIn)
                                break;
                        }
                    }
                    Console.WriteLine();
                    break;

                case "q":
                    ClearConsole();
                    return false;

                case "keygen":
                    string key = EncryptionUtil.GenerateSecureKey();
                    Console.Write(YELLOW);
                    for (int i = 0; i < key.Length; i++)
                        Console.Write("-");

                    Console.WriteLine("\n" + BLUE + key);

                    Console.Write(YELLOW);
                    for (int i = 0; i < key.Length; i++)
                        Console.Write("-");
                    Console.Write("\n\n" + RESET);

                    break;

                default:
                    Console.WriteLine(RED + $"{input} is not a valid command\n");
                    break;
            }

            return true;
        }

        internal void SPMUserIndicator(string username)
        {
            Console.Write(BLUE + "[SPM~" + RESET + YELLOW + $"{username}" + RED + "~" + BLUE + "] " + RESET);
        }

        public bool LoginOptions(string username, string input)
        {

            try
            {
                var serviceRepoAccessor = new ServiceRepositoryAccessor();
                var userRepositoryAcessor = new UserRepositoryAcessor();

                string serviceName = string.Empty;
                if (input.StartsWith("out", StringComparison.Ordinal))
                {
                    // Console.WriteLine("\n\t" + GoodbyeMessage(username) + "\n");
                    TypeMessage(GoodbyeMessage(username));
                    Console.CursorVisible = false;
                    string[] spinner = { "/", "-", "\\", "|" }; // Characters for spinning animation
                    for (int i = 0; i < 20; i++)
                    {
                        if (i < 8)
                        {
                            Console.Write(GREEN + spinner[i % spinner.Length] + RESET);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
                        }
                        if (i > 8 && i < 14)
                        {
                            Console.Write(YELLOW + spinner[i % spinner.Length] + RESET);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
                        }
                        else
                        {
                            Console.Write(RED + spinner[i % spinner.Length] + RESET);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
                        }

                        Thread.Sleep(100); // Adjust sleep time for speed of animation

                    }
                    Console.WriteLine();
                    Console.CursorVisible = true;
                    return false;

                }
                else if (input.Equals(""))
                {
                    return true;
                }
                else if (input.StartsWith("cl", StringComparison.Ordinal))
                {
                    ClearConsole();
                    return true;
                }
                else if (input.StartsWith("h", StringComparison.Ordinal))
                {
                    ShowHelpCommands();
                    return true;
                }
                else if (input.StartsWith("new", StringComparison.Ordinal))
                {
                    try
                    {
                        serviceName = input.Substring(4).Trim();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(RED + "Empty service name\n" + RESET);
                        return true;
                    }

                    Console.WriteLine(YELLOW + "-----New Service-----" + RESET);

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Check service exists
                    if (serviceRepoAccessor.ServiceExistsByUserID(serviceName, userID))
                    {
                        Console.WriteLine(RED + $"\"{serviceName}\" already exists\n" + RESET);
                        return true;
                    }

                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);
                    // Prompt service password
                    PromptForPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();
                    PromptForRepeatPassword();
                    string repeatPassword = HashUtil.ReadPassword();


                    if (repeatPassword.Equals(password))
                    {
                        var service = new Service(serviceName, password);

                        // Check if service exists for this user
                        if (serviceRepoAccessor.ServiceExistsByUserID(service, userID))
                            throw new SimpleException("Service already exists, use 'upd' command instead");

                        // Add to DB
                        if (!serviceRepoAccessor.Add(service, userID))
                            throw new SimpleException($"Error Processing service: \"{service.Name}\"");

                        // Display Process
                        Console.WriteLine(BLUE + "Encrypting.." + RESET);
                        Thread.Sleep(200);
                        Console.Write(BLUE + "Saving.." + RESET);
                        Thread.Sleep(200);
                        Console.Write(GREEN + "Saved!" + RESET);
                        Console.WriteLine();
                    }
                    else
                        throw new SimpleException("Password Mismatch");

                    return true;
                }
                else if (input.StartsWith("upd", StringComparison.Ordinal))
                {
                    // string serviceName = input.Substring(4).Trim();
                    try
                    {
                        serviceName = input.Substring(4).Trim();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(RED + "Empty service name\n" + RESET);
                        return true;
                    }

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Check service exists
                    if (!serviceRepoAccessor.ServiceExistsByUserID(serviceName, userID))
                    {
                        Console.WriteLine(RED + $"\"{serviceName}\" does not exist\n" + RESET);
                        return true;
                    }

                    Console.WriteLine(YELLOW + "-----Update Service-----" + RESET);
                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);

                    // Prompt service password
                    PromptForPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();
                    PromptForRepeatPassword();
                    string repeatPassword = HashUtil.ReadPassword();

                    if (repeatPassword.Equals(password))
                    {

                        var service = new Service(serviceName, password);


                        // Add to DB
                        if (!serviceRepoAccessor.UpdateServiceEncryption(service, userID))
                            throw new SimpleException($"Error updating: \"{service.Name}\"");


                        // Display Process
                        Console.WriteLine("\n" + BLUE + "Encrypting.." + RESET);
                        Thread.Sleep(200);
                        Console.Write(BLUE + "Updating.." + RESET);
                        Thread.Sleep(200);
                        Console.Write(GREEN + "Updated!" + RESET);
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine("\n" + RED + "Password mismatch" + RESET);


                    return true;
                }
                else if (input.StartsWith("del", StringComparison.Ordinal))
                {
                    try
                    {
                        serviceName = input.Substring(4).Trim();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(RED + "Empty service name\n" + RESET);
                        return true;
                    }

                    Console.WriteLine(YELLOW + "-----Delete Service-----" + RESET);

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Check service exists
                    if (!serviceRepoAccessor.ServiceExistsByUserID(serviceName, userID))
                    {
                        Console.WriteLine(RED + $"\"{serviceName}\" does not exist\n" + RESET);
                        return true;
                    }

                    Console.WriteLine(GREEN + "Service: " + BLUE + $"{serviceName}" + RESET);
                    // Prompt master password
                    PromptForMasterPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine(RESET);


                    // Get master hash
                    var (isHash, hash) = userRepositoryAcessor.GetPasswordHash(userID);

                    if (!isHash)
                        throw new SimpleException("Incorrect Master Password");

                    // Verify master password
                    if (!UserUtil.VerifyHashedPassword(username, password, hash))
                        throw new SimpleException("Incorrect Master Password");

                    // Delete service
                    bool isDeleted = serviceRepoAccessor.DeleteByServiceName(serviceName, userID);

                    if (isDeleted)
                    {
                        // Display Process
                        Console.Write(BLUE + "Deleting.." + RESET);
                        Thread.Sleep(200);
                        Console.Write(GREEN + "Deleted!" + RESET);
                        Console.WriteLine(YELLOW + "------------------------\n" + RESET);

                    }
                    else
                    {
                        Console.WriteLine(RED + $"Could not delete service \"{serviceName}\"" + RESET);
                    }

                    return true;
                }
                else if (input.StartsWith("lsp", StringComparison.Ordinal))
                {
                    try
                    {
                        serviceName = input.Substring(4).Trim();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(RED + "Empty service name\n" + RESET);
                        return true;
                    }

                    Console.WriteLine(YELLOW + "-----Password Display-----" + RESET);

                    // Prompt master password
                    PromptForMasterPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Get master hash
                    var (isHash, hash) = userRepositoryAcessor.GetPasswordHash(userID);

                    if (!isHash)
                        throw new SimpleException("Incorrect Master Password");

                    // Verify master password
                    if (!UserUtil.VerifyHashedPassword(username, password, hash))
                        throw new SimpleException("Incorrect Master Password");


                    // Check service exists for user
                    if (!serviceRepoAccessor.ServiceNameExistsByUserID(serviceName, userID))
                        throw new SimpleException($"Service does not exist: \"{serviceName}\"");

                    // Get the encrypted password
                    var (isEncrypted, encrypted) = serviceRepoAccessor.GetEncryptedByUserID(serviceName, userID);

                    if (!isEncrypted)
                        throw new SimpleException("Could not get password");

                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);
                    Console.WriteLine(GREEN + $"Password: {EncryptionUtil.DecryptString(encrypted)}\n");


                    return true;
                }
                else if (input.StartsWith("lsn", StringComparison.Ordinal))
                {

                    // Get all service names by userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Get all services
                    var servicesList = serviceRepoAccessor.GetAllServicesByUserID(userID);

                    // Verify list
                    if (servicesList.Count == 0)
                    {
                        Console.WriteLine(RED + "No services available" + RESET);
                        return true;
                    }

                    Console.WriteLine(YELLOW + "-----Service List-----" + RESET);
                    foreach (var pair in servicesList)
                    {
                        Console.WriteLine(BLUE + $"{pair.Item1.PadRight(15)}: {pair.Item2.PadRight(20)}" + RESET);
                    }
                    Console.WriteLine(YELLOW + "----------------------\n" + RESET);

                    return true;
                }
                else if (input.StartsWith("kll", StringComparison.Ordinal))
                {
                    Console.WriteLine(YELLOW + "-----Delete User-----" + RESET);

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    if (userID == -1)
                        throw new SimpleException("User not found");


                    // Prompt master password
                    PromptForMasterPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();

                    // Get master hash
                    var (isHash, hash) = userRepositoryAcessor.GetPasswordHash(userID);

                    if (!isHash)
                        throw new SimpleException("Incorrect Master Password");

                    // Verify master password
                    if (!UserUtil.VerifyHashedPassword(username, password, hash))
                        throw new SimpleException("Incorrect Master Password");

                    // Delete user and all associated data
                    bool isDeleted = userRepositoryAcessor.DeleteByUSerID(userID);


                    // Display Process
                    Console.Write(BLUE + "Permanently Deleting User.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(RED + "Deleted!" + RESET);
                    Console.WriteLine();

                    if (isDeleted)
                        return false;
                    else
                        return true;


                }
                else if (input.StartsWith("bup", StringComparison.Ordinal))
                {
                    Console.Write(YELLOW + "-----Database Backup-----" + RESET);
                    if (!userRepositoryAcessor.BackupWithScript())
                        throw new SimpleException("Could not backup passwords");

                    Console.Write(BLUE + "Creating backup.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(GREEN + "Backup saved!" + RESET + "\n");
                    Thread.Sleep(200);
                    // Console.WriteLine("\n" + YELLOW + $"Location: {Environment.GetEnvironmentVariable("MYSQL_BACKUP_PATH")}" + RESET);
                    Console.WriteLine(YELLOW + "-------------------------" + RESET);
                    return true;

                }
                else if (input.StartsWith("rev", StringComparison.Ordinal))
                {
                    Console.WriteLine(YELLOW + "-----Database Restore-----" + RESET);
                    var backupsList = userRepositoryAcessor.GetAllBackups();

                    if (backupsList.Length <= 0)
                        throw new SimpleException("No backups exist.");

                    foreach (var backup in backupsList)
                    {
                        Console.WriteLine(CYAN + $"{backup}" + RESET);
                    }
                    Console.WriteLine(YELLOW + "--------------------------" + RESET);

                    // Ask for file to backup with
                    Console.WriteLine(BLUE + "Paste the file name to revert back or q to exit." + CYAN);
                    Console.Write(YELLOW + "|" + CYAN);

                    string fileName;
                    try
                    {
                        fileName = GetSensitiveConsoleText();

                        if (fileName.Equals("q"))
                            return true;
                    }
                    catch (SimpleException ex)
                    {
                        HandleException(ex);
                        return true;
                    }
                    catch (Exception)
                    {
                        return true;
                    }

                    Console.Write(YELLOW + "--------------------------" + RESET);

                    if (!userRepositoryAcessor.RestoreWithScript(fileName))
                        throw new SimpleException("Error reverting datbase");

                    Console.Write(GREEN + "Database restored successfully.\n" + RESET);

                    if (!userRepositoryAcessor.UsernameExists(username))
                    {
                        Console.WriteLine(RED + $"Logging out, \"{username}\" was created after this restore" + RESET);
                        return false;
                    }
                    Console.WriteLine(YELLOW + "--------------------------" + RESET);

                    return true;

                }
                else if (input.StartsWith("keygen", StringComparison.Ordinal))
                {
                    string key = EncryptionUtil.GenerateSecureKey();
                    Console.Write(YELLOW);
                    for (int i = 0; i < key.Length; i++)
                        Console.Write("-");

                    Console.WriteLine("\n" + BLUE + key);

                    Console.Write(YELLOW);
                    for (int i = 0; i < key.Length; i++)
                        Console.Write("-");
                    Console.Write("\n\n" + RESET);

                    return true;
                }
                else
                {
                    throw new SimpleException(RED + $"{input} is not a command\n" + RESET);
                }
            }
            catch (SimpleException ex)
            {
                // Console.WriteLine("\n" + RED + ex + RESET);
                HandleException(ex);

                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return true;
            }

        }

        internal static (bool, string) Login()
        {
            var userRepoAcess = new UserRepositoryAcessor();

            try
            {
                Console.WriteLine(YELLOW + "-----User Login-----" + RESET);

                // Get username
                PromptForUsername();
                Console.Write(BLUE);
                string username = GetSensitiveUsernameConsoleText();
                Console.Write(RESET);
                if (!userRepoAcess.UsernameExists(username))
                    throw new SimpleException($"Username \"{username}\" does not exist");

                // Get comparison password
                PromptForPassword();
                string password = HashUtil.ReadPassword();

                // Get user hash from DB
                int userID = userRepoAcess.GetUserIDByUserName(username);
                var (isHash, hash) = userRepoAcess.GetPasswordHash(userID);

                if (isHash)
                {
                    var (isValid, name) = (UserUtil.VerifyHashedPassword(username, password, hash), username);

                    if (!isValid)
                    {
                        // Console.WriteLine("\n" + WrongPasswordMessage());
                        TypeMessage(WrongPasswordMessage() + "\n");
                        return (false, name);
                    }

                    Console.WriteLine(YELLOW + "\n--------------------" + RESET);

                    return (true, name);
                }

                throw new SimpleException("User does not exist");
                // return (false, username);
            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return (false, string.Empty);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return (false, string.Empty);
            }
        }

        internal static void HandleException(Exception ex)
        {
            // Log full exception details to a secure log file
            File.WriteAllText("error_log.txt", ex.ToString());

            // Display simplified message to the user
            Console.WriteLine(RED + $"{ex.Message}" + RESET);

            // Console.WriteLine(RED + "StackTrace: [Stack trace hidden]" + RESET);
        }

        internal static void ShowHelpCommands()
        {
            Console.WriteLine(
                YELLOW + "keygen" + RESET + " :" + BLUE + " Generate a random secure key.\n" +
                YELLOW + "cr" + RESET + " :" + BLUE + " Create a new user profile. Prompts user for a unique username and a master password\n" +
                YELLOW + "cl" + RESET + " :" + BLUE + " Clear console\n" +
                YELLOW + "lg" + RESET + " :" + BLUE + " Login to a user profile. Allows access to saved service passwords. Other commands are available\n" +
                      CYAN + "\tnew" + RED + " {serivce name}" + RESET + " :" + BLUE + " Add a new service password\n" +
                      CYAN + "\tupd" + RED + " {serivce name}" + RESET + " :" + BLUE + " Update a current service password\n" +
                      CYAN + "\tdel" + RED + " {serivce name}" + RESET + " :" + BLUE + " Delete service from profile\n" +
                      CYAN + "\tlsp" + RED + " {serivce name}" + RESET + " :" + BLUE + " List user password for 'service name'\n" +
                      CYAN + "\tlsn" + RESET + " :" + BLUE + " List all user service names\n" +
                      CYAN + "\tbup" + RESET + " :" + BLUE + " Backup Database\n" +
                      CYAN + "\trev" + RESET + " :" + BLUE + " Revert to a recent databse version\n" +
                      CYAN + "\tout" + RESET + " :" + BLUE + " Log out of profile\n" +
                      CYAN + "\tkll" + RESET + " :" + BLUE + " Delete User and all passwords\n" +
                YELLOW + "lu" + RESET + " :" + BLUE + " List all users.\n" +
                YELLOW + " h" + RESET + " :" + BLUE + " Display commands\n" +
                YELLOW + " q" + RESET + " :" + BLUE + " Exit program\n"

            );
        }
        internal static bool isValidInput(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (input.Length < 1 || input.Length > 128)
                return false;

            return true;
        }

        internal static bool isValidPassord(string password)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    throw new SimpleException("INVALID PASSWORD: (empty)");

                bool isValid = UserUtil.IsValidPassword(password);

                if (!isValid)
                    throw new SimpleException($"INVALID PASSWORD: length({password.Length}) -> [Min: 8, Max: 128]");

                return true;

            }
            catch (SimpleException ex)
            {
                HandleException(ex);
                return false;
            }
            catch (Exception ex)
            {
                // Console.WriteLine(RED + ex.Message + RESET);
                HandleException(ex);
                return false;
            }
        }

        internal static bool isValidUsername(string username)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("INVALID USERNAME: (empty)");

                bool isValid = UserUtil.IsValidUsername(username);

                if (!isValid)
                    throw new ArgumentException($"INVALID USERNAME: length({username.Length}) -> [Min: 3, Max: 32]");

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
                return false;
            }


        }

        internal static bool isValidSPMNonSensitiveInput(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            if (input.Length < 0 || input.Length > 16)
                return false;

            return true;
        }

        internal static string WelcomeMessage(string username)
        {
            var welcomes = new string[]
                {
                    YELLOW + "Welcome, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Hello, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Hi, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Greetings, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Welcome back, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Hi there, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Hey pal, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Good to see you, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Welcome aboard, " + BLUE + $"{username}!" + RESET,
                    YELLOW + "Great, it's " + BLUE + $"{username} " + YELLOW + "again. Try not to break anything this time." + RESET,
                    YELLOW + "Oh, it's you again. What do you want this time, " + BLUE + $"{username}" + YELLOW + "?" + RESET,
                    YELLOW + "Look who decided to grace us with their presence. Welcome back, " + BLUE + $"{username}" + YELLOW + "." + RESET,
                    YELLOW + "Oh joy, it's " + BLUE + $"{username}" + YELLOW + ". What a delightful surprise." + RESET,
                    YELLOW + "Ahoy, " + BLUE + $"{username}" + YELLOW + "... Beware the whispers of the forgotten souls that linger here." + RESET,
                    YELLOW + "Welcome, " + BLUE + $"{username}" + YELLOW + "! You're here, so I guess we're stuck with you. Enjoy your stay!" + RESET,
                    YELLOW + "Greetings, " + BLUE + $"{username}" + YELLOW + "! We accept all kinds here, even the ones who can't read signs." + RESET,
                    YELLOW + "Welcome, " + BLUE + $"{username}" + YELLOW + "... Prepare to lose yourself in the labyrinth of despair." + RESET,
                    YELLOW + "Hi, " + BLUE + $"{username}" + YELLOW + "! Great to see you!" + RESET
                };

            Random random = new Random();
            int index = random.Next(welcomes.Length);

            string message = welcomes[index];

            return message;
        }
        internal static string GoodbyeMessage(string username)
        {
            var goodbyes = new string[]
            {
                YELLOW + "Goodbye, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "See you later, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Take care, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Farewell, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Until next time, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Bye, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Catch you later, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "So long, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Have a great day, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Stay safe, " + BLUE + $"{username}!"+ RESET,
                YELLOW + "Goodbye, " + BLUE + $"{username}!" + YELLOW + "May the darkness shield your secrets." + RESET,
            };

            Random random = new Random();
            int index = random.Next(goodbyes.Length);

            string message = goodbyes[index];

            return message;
        }

        internal static string WrongPasswordMessage()
        {
            string[] messages =
            {
                YELLOW + "Oops! Wrong password. Did you forget it again?" + RESET,
                YELLOW + "Nope, try again! Maybe the caps lock is on?" + RESET,
                YELLOW + "That's not it. Did your cat walk over the keyboard?" + RESET,
                YELLOW + "Access denied. Are you sure you're you?" + RESET,
                YELLOW + "Nice try, but no. Time to double-check your notes?" + RESET,
                YELLOW + "Password fail! Maybe it's time for a break?" + RESET,
                YELLOW + "Incorrect password. Did you change it and forget?" + RESET,
                YELLOW + "Wrong again. Did you mean to type something else?" + RESET,
                YELLOW + "Denied! Perhaps you need a little more coffee?" + RESET,
                YELLOW + "No luck. Are you testing our security?" + RESET,
                YELLOW + "Oops! Wrong password. Remember, every mistake is a step towards learning." + RESET,
                YELLOW + "No worries! Keep trying. Success is just around the corner." + RESET,
                YELLOW + "Not quite there! Persistence is key. Keep going!" + RESET,
                YELLOW + "Incorrect password, but don't lose heart! You're making progress with every attempt." + RESET,
                YELLOW + "Brute force, huh? Impressive! But our server needs a breather." + RESET,
                YELLOW + "Brute force detected! Maybe try knocking next time?" + RESET,
                YELLOW + "Oh, you're trying to break in? Sorry, our system prefers sweet talk over brute force." + RESET,
                YELLOW + "Unauthorized access attempt! Looks like someone's keyboard is on fire from all the furious typing." + RESET,
                YELLOW + "Hackerman in action! But don't worry, our security is as tough as grandma's cookies." + RESET,
                YELLOW + "Yawn! That password is about as exciting as watching paint dry." + RESET,
                YELLOW + "Hmm... Did you choose that password while counting sheep?" + RESET,
             };

            Random random = new Random();
            int index = random.Next(messages.Length);

            string message = messages[index];

            return message;
        }
        public static string ProgramGoodbyeMessage()
        {
            string[] messages =
            {
                YELLOW + "¡Hasta luego! Que tengas un buen día." + RESET,
                YELLOW + "Goodbye! Have a great day." + RESET,
                YELLOW + "See you later! Take care." + RESET,
                YELLOW + "Farewell! Until we meet again." + RESET,
                YELLOW + "Bye for now! Stay safe" + RESET,
                YELLOW + "Take care! See you soon." + RESET,
                YELLOW + "Smell ya later, alligator!" + RESET,
                YELLOW + "Goodbye, mortal. Beware the shadows." + RESET,
                YELLOW + "Farewell, traveler. The darkness awaits." + RESET,
                YELLOW + "Bye for now. Remember, the monsters come out at night." + RESET,
                YELLOW + "See you never... in this world or the next." + RESET,
                YELLOW + "Goodbye for now. Remember to cherish every moment." + RESET,
                YELLOW + "Farewell, but remember, every ending is a new beginning." + RESET,
                YELLOW + "Until we meet again. Let the lessons of today guide you into tomorrow." + RESET,
                YELLOW + "Farewell! Remember, every setback is a setup for a comeback." + RESET,
                YELLOW + "Bye for now! You're stronger than you think. Keep going!" + RESET,
                YELLOW + "See you later! Believe in yourself and your dreams." + RESET,
                YELLOW + "Bye." + RESET,
             };

            Random random = new Random();
            int index = random.Next(messages.Length);

            string message = messages[index];

            return message;
        }

        public static void TypeMessage(string message)
        {
            Console.Write("\n\t");
            foreach (char c in message)
            {
                Console.Write(c);
                if (message.Length < 20)
                    Thread.Sleep(65); // Adjust the delay as needed
                else
                    Thread.Sleep(30);
            }

            Console.WriteLine(); // Move to the next line after typing the text
        }
        public static void TypeMessageWithTime(string message, int milis)
        {
            Console.Write("\n");
            foreach (char c in message)
            {
                Console.Write(c);
            }
            Thread.Sleep(20); // Adjust the delay as needed

        }
    }
}
