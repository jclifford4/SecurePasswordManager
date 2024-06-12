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
                    Console.WriteLine();
                }

                Prompt.SPMIndicator();
                Console.Write(Prompt.YELLOW);

                string? input = Prompt.GetNonSensitiveConsoleText();

                if (input != null)
                    running = prompt.ProgramOptions(input);

                if (!running)
                    break;

            }
            Prompt.TypeMessage(Prompt.ProgramGoodbyeMessage() + "\n\n\n\n\n");
            Console.Write(Prompt.RESET);
            // Thread.Sleep(4000);
        }
    }
}
