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
                Prompt.GetNonSensitiveConsoleText();
                Prompt.PromptForUserName();
                Prompt.GetSensitiveUsernameConsoleText();
                Prompt.PromptForPassword();
                Prompt.GetSensitivePasswordConsoleText();
                Prompt.PromptForRepeatPassword();
                Prompt.GetSensitivePasswordConsoleText();


            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
