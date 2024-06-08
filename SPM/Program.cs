using PasswordUtility;
using UserUtility;
using HashUtility;
using Users;
using UserRepository;
using ProgramPrompts;

namespace Main
{
    public class Program
    {

        // TODO: First time startup to set env vars
        // TODO: Add backup command for user after login
        // TODO: Tidy error messages for user
        // TODO: Clean up code
        public static void Main(string[] args)
        {
            var prompt = new Prompt();
            bool running = true;
            bool isFirstStarup = true;
            Prompt.ClearConsole();
            while (running)
            {
                if (isFirstStarup)
                {
                    Prompt.StartUp();
                    Prompt.Help();
                    isFirstStarup = false;
                }

                Prompt.SPMIndicator();
                Console.Write(Prompt.YELLOW);

                string? input = Prompt.GetNonSensitiveConsoleText();

                if (input != null)
                    running = prompt.ProgramOptions(input);

                if (!running)
                    break;

            }
            Console.Write(Prompt.RESET);
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
