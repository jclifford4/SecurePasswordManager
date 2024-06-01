using UserUtility;
namespace ProgramPrompts

{
    public static class Prompt
    {

        // ANSI escape code constants
        private const string RESET = "\u001b[0m";
        private const string RED = "\u001b[31m";
        private const string GREEN = "\u001b[32m";
        private const string YELLOW = "\u001b[33m";
        private const string BLUE = "\u001b[34m";

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

        public static void PromptForUserName()
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

            Console.Write(GREEN + "New Password: " + RESET);
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
        public static string? GetSensitivePasswordConsoleText()
        {
            try
            {
                string? input = Console.ReadLine();
                if (isValidPassord(input))
                    return input;

                throw new ArgumentException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(RED + ex.Message + RESET);
                return "error";
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
