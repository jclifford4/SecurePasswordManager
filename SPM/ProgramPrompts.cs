using EncryptionUtility;
using HashUtility;
using Microsoft.AspNetCore.Identity;
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

        /*
            User Prompts
        */
        public static void StartUp()
        {
            Console.WriteLine(YELLOW + "[---Secure Password Manager---]" + RESET);
        }

        public static void Help()
        {
            Console.WriteLine("(h -help q -quit)");
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

                throw new ArgumentException("Invalid Input");
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
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

                throw new ArgumentException();

            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
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

                throw new ArgumentException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
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
                    throw new ArgumentException($"Invalid Input: length({input.Length}) -> [Min: 0, Max: 16]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
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
                    throw new Exception("Username Error: may have illegal characters");

                string password = CreateNewMasterPassword();
                if (password.Equals("error"))
                    throw new Exception("Password Error");


                User user = new User(username, password);
                return userRepoAcess.Add(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
                return false;
            }
        }

        //TODO: More commands.
        public bool ProgramOptions(string input)
        {
            switch (input)
            {
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
                        Console.WriteLine(GREEN + "\nLogged in!" + RESET);
                        Thread.Sleep(500);
                        ClearConsole();

                        while (isLoggedIn)
                        {
                            SPMUserIndicator(username);
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

                default:
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
                    Console.WriteLine(RED + $"Logged out!" + RESET);
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
                    serviceName = input.Substring(4).Trim();
                    Console.WriteLine(YELLOW + "-----New Service-----" + RESET);
                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);

                    // Prompt service password
                    PromptForPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();
                    PromptForRepeatPassword();
                    string repeatPassword = HashUtil.ReadPassword();

                    // Create service
                    Service service;
                    if (repeatPassword.Equals(password))
                        service = new Service(serviceName, password);
                    else
                        throw new ArgumentException("Password mismatch");

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Check if service exists for this user
                    if (serviceRepoAccessor.ServiceExistsByUserID(service, userID))
                        throw new ArgumentException("Service already exists, use 'upd' command instead");

                    // Add to DB
                    if (!serviceRepoAccessor.Add(service, userID))
                        throw new ArgumentException($"Error Processing service: \"{service.Name}\"");

                    // Display Process
                    Console.Write(BLUE + "Encrypting.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(BLUE + "Saving.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(GREEN + "Saved!" + RESET);
                    Console.WriteLine();
                    return true;
                }
                else if (input.StartsWith("upd", StringComparison.Ordinal))
                {
                    serviceName = input.Substring(4).Trim();
                    Console.WriteLine(YELLOW + "-----Update Service-----" + RESET);
                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);

                    // Prompt service password
                    PromptForPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();
                    PromptForRepeatPassword();
                    string repeatPassword = HashUtil.ReadPassword();

                    // Create service
                    Service service;
                    if (repeatPassword.Equals(password))
                        service = new Service(serviceName, password);
                    else
                        throw new ArgumentException("Password mismatch");

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Check if service exists for this user
                    if (!serviceRepoAccessor.ServiceExistsByUserID(service, userID))
                        throw new ArgumentException("Service does not exists, use 'new' command instead");

                    // Add to DB
                    if (!serviceRepoAccessor.UpdateServiceEncryption(service, userID))
                        throw new ArgumentException($"Error Processing service: \"{service.Name}\"");

                    // Display Process
                    Console.Write(BLUE + "Encrypting.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(BLUE + "Updating.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(GREEN + "Updated!" + RESET);
                    Console.WriteLine();
                    return true;
                }
                else if (input.StartsWith("del", StringComparison.Ordinal))
                {
                    serviceName = input.Substring(4).Trim();
                    Console.WriteLine(YELLOW + "-----Delete Service-----" + RESET);
                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);

                    // Prompt master password
                    PromptForMasterPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    // Get master hash
                    var (isHash, hash) = userRepositoryAcessor.GetPasswordHash(userID);

                    if (!isHash)
                        throw new ArgumentException("Incorrect Master Password");

                    // Verify master password
                    if (!UserUtil.VerifyHashedPassword(username, password, hash))
                        throw new ArgumentException("Incorrect Master Password");

                    // Delete service
                    bool isDeleted = serviceRepoAccessor.DeleteByServiceName(serviceName, userID);

                    // Display Process
                    Console.Write(BLUE + "Deleting.." + RESET);
                    Thread.Sleep(200);
                    Console.Write(GREEN + "Deleted!" + RESET);
                    Console.WriteLine();
                    return isDeleted;
                }
                else if (input.StartsWith("lsp", StringComparison.Ordinal))
                {
                    serviceName = input.Substring(4).Trim();
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
                        throw new ArgumentException("Incorrect Master Password");

                    // Verify master password
                    if (!UserUtil.VerifyHashedPassword(username, password, hash))
                        throw new ArgumentException("Incorrect Master Password");


                    // Check service exists for user
                    if (!serviceRepoAccessor.ServiceNameExistsByUserID(serviceName, userID))
                        throw new ArgumentException($"Service does not exist: {serviceName}");

                    // Get the encrypted password
                    var (isEncrypted, encrypted) = serviceRepoAccessor.GetEncryptedByUserID(serviceName, userID);

                    if (!isEncrypted)
                        throw new ArgumentException("Could not get password");

                    Console.WriteLine(GREEN + $"Service: {serviceName}" + RESET);
                    Console.WriteLine(GREEN + $"Password: {EncryptionUtil.DecryptString(encrypted)}");


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
                        throw new ArgumentException("Error: Empty service list");
                    Console.WriteLine(YELLOW + "-----Service List-----" + RESET);
                    foreach (var pair in servicesList)
                    {
                        Console.WriteLine(BLUE + $"{pair.Item1.PadRight(15)}: {pair.Item2.PadRight(20)}" + RESET);
                    }
                    Console.WriteLine(YELLOW + "----------------------" + RESET);
                    return true;
                }
                else if (input.StartsWith("bup", StringComparison.Ordinal))
                {
                    //TODO:Implement DB backup
                    return true;
                }
                else if (input.StartsWith("rev", StringComparison.Ordinal))
                {
                    //TODO:Implement DB revert
                    return true;
                }
                else if (input.StartsWith("kll", StringComparison.Ordinal))
                {
                    Console.WriteLine(YELLOW + "-----Delete User-----" + RESET);

                    // Get userID
                    int userID = userRepositoryAcessor.GetUserIDByUserName(username);

                    if (userID == -1)
                        throw new ArgumentException("User not found");


                    // Prompt master password
                    PromptForMasterPassword();
                    string password = HashUtil.ReadPassword();
                    Console.WriteLine();

                    // Get master hash
                    var (isHash, hash) = userRepositoryAcessor.GetPasswordHash(userID);

                    if (!isHash)
                        throw new ArgumentException("Incorrect Master Password");

                    // Verify master password
                    if (!UserUtil.VerifyHashedPassword(username, password, hash))
                        throw new ArgumentException("Incorrect Master Password");

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
                else
                {
                    throw new Exception(RED + "Incomplete command" + RESET);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex + RESET);
                return true;
            }

        }

        internal static (bool, string) Login()
        {
            var userRepoAcess = new UserRepositoryAcessor();

            try
            {
                // Get username
                PromptForUsername();
                string username = GetSensitiveUsernameConsoleText();
                if (!userRepoAcess.UsernameExists(username))
                    throw new Exception();

                // Get comparison password
                PromptForPassword();
                string password = HashUtil.ReadPassword();

                // Get user hash from DB
                int userID = userRepoAcess.GetUserIDByUserName(username);
                var (isHash, hash) = userRepoAcess.GetPasswordHash(userID);

                if (isHash)
                {
                    return (UserUtil.VerifyHashedPassword(username, password, hash), username);
                }

                return (false, username);
            }
            catch (Exception)
            {
                Console.WriteLine(RED + "Login failed: " + RESET);
                return (false, string.Empty);
            }
        }

        internal static void ShowHelpCommands()
        {
            Console.WriteLine(
                YELLOW + "cr" + RESET + " :" + BLUE + " Create a new user profile. Prompts user for a unique username and a master password\n" +
                YELLOW + "cl" + RESET + " :" + BLUE + " Clear console\n" +
                YELLOW + "lg" + RESET + " :" + BLUE + " Login to a user profile. Allows access to saved service passwords. Other commands are available\n" +
                      YELLOW + "\tnew" + RED + " {serivce name}" + RESET + " :" + BLUE + " Add a new service password\n" +
                      YELLOW + "\tupd" + RED + " {serivce name}" + RESET + " :" + BLUE + " Update a current service password\n" +
                      YELLOW + "\tdel" + RED + " {serivce name}" + RESET + " :" + BLUE + " Delete service from profile\n" +
                      YELLOW + "\tlsp" + RED + " {serivce name}" + RESET + " :" + BLUE + " List user password for 'service name'\n" +
                      YELLOW + "\tlsn" + RESET + " :" + BLUE + " List all user service names\n" +
                      YELLOW + "\tbup" + RESET + " :" + BLUE + " Backup Database\n" +
                      YELLOW + "\trev" + RESET + " :" + BLUE + " Revert to most recent database save file\n" +
                      YELLOW + "\tout" + RESET + " :" + BLUE + " Log out of profile\n" +
                      YELLOW + "\tkll" + RESET + " :" + BLUE + " Delete User and all passwords\n" +
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
                    throw new ArgumentException("INVALID PASSWORD: (empty)");

                bool isValid = UserUtil.IsValidPassword(password);

                if (!isValid)
                    throw new ArgumentException($"INVALID PASSWORD: length({password.Length}) -> [Min: 8, Max: 128]");

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
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




    }
}
