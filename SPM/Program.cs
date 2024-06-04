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
        public static void Main(string[] args)
        {
            var userRepoAcess = new UserRepositoryAcessor();
            var prompt = new Prompt();

            bool running = true;
            bool isFirstStarup = true;
            while (running)
            {
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
