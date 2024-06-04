using HashUtility;
using Microsoft.AspNetCore.Identity;
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
            Console.Write(BLUE + "[SPM]> " + RESET);
        }
        public static void PromptForPassword()
        {
            Console.Write(GREEN + "Password: " + RESET);
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

                case "lg":
                    ClearConsole();
                    var (isLoggedIn, username) = Login();
                    if (isLoggedIn)
                    {
                        Console.WriteLine(GREEN + "Logged in!" + RESET);
                        while (isLoggedIn)
                        {
                            SPMUserIndicator(username);
                            isLoggedIn = LoginOptions(GetNonSensitiveConsoleText());

                            if (!isLoggedIn)
                                break;
                        }
                    }
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
            Console.Write(BLUE + "[SPM" + RESET + YELLOW + $"{username}" + BLUE + "]>" + RESET);
        }

        //TODO: Get all user commands working.
        public bool LoginOptions(string input)
        {
            string data = string.Empty;
            if (input.StartsWith("new", StringComparison.Ordinal))
            {
                data = input.Substring(4).Trim();
                Console.WriteLine(GREEN + $" {data}" + RESET);
                return true;
            }
            else if (input.StartsWith("out", StringComparison.Ordinal))
            {
                data = input.Substring(4).Trim();
                Console.WriteLine(RED + $" Logged out" + RESET);
                return false;

            }

            return false;
        }

        internal static (bool, string) Login()
        {
            var userRepoAcess = new UserRepositoryAcessor();
            PromptForUsername();
            try
            {


                string username = GetSensitiveUsernameConsoleText();
                if (!userRepoAcess.UsernameExists(username))
                    throw new Exception();

                string passwordHash = UserUtil.HashPassword(username, HashUtil.ReadPassword());
                if (!userRepoAcess.MasterHashExists(passwordHash))
                    throw new Exception();


                return (true, username);
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
                YELLOW + "cr" + RESET + " :" + BLUE + " Create a new user profile. Prompts user for a unique username and a master password.\n" +
                YELLOW + "cl" + RESET + " :" + BLUE + " Clear console.\n" +
                YELLOW + "lg" + RESET + " :" + BLUE + " Login to a user profile. Allows access to saved service passwords. Other commands are available.\n" +
                      YELLOW + "\tnew" + RED + " {serivce name}" + RESET + " :" + BLUE + " Add a new service password.\n" +
                      YELLOW + "\tupd" + RED + " {serivce name}" + RESET + " :" + BLUE + " Update a current service password.\n" +
                      YELLOW + "\tdel" + RED + " {serivce name}" + RESET + " :" + BLUE + " Delete service from profile.\n" +
                      YELLOW + "\tlsp" + RED + " {serivce name}" + RESET + " :" + BLUE + " List user password for 'service name'\n" +
                      YELLOW + "\tlse" + RESET + " :" + BLUE + " List user encrypted passwords\n" +
                      YELLOW + "\tlsn" + RESET + " :" + BLUE + " List all user service names\n" +
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
