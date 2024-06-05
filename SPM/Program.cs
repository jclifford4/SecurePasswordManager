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
            while (running)
            {
                // Console.WriteLine(Environment.GetEnvironmentVariable("ENCRYPTION_KEY"));
                // Console.WriteLine(Environment.GetEnvironmentVariable("HOST"));
                // Console.WriteLine(Environment.GetEnvironmentVariable("USER"));
                // Console.WriteLine(Environment.GetEnvironmentVariable("PASSWORD"));
                // Console.WriteLine(Environment.GetEnvironmentVariable("DATABASE"));
                // Console.WriteLine(Environment.GetEnvironmentVariable("BACKUP_PATH"));

                if (isFirstStarup)
                {
                    Prompt.StartUp();
                    isFirstStarup = false;
                }

                Prompt.SPMIndicator();
                running = prompt.ProgramOptions(Prompt.GetNonSensitiveConsoleText());

                if (!running)
                    break;

            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
