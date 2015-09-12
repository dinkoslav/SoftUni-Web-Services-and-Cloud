namespace Battleships.ConsoleClient
{
    using System;

    class ConsoleClient
    {
        static void Main(string[] args)
        {
            CommandProcessor commandProcessor = new CommandProcessor();
            bool gameIsRunning = true;

            Console.WriteLine("Commands: $register, $login, $create-game, $join-game, $play, $exit");

            while (gameIsRunning)
            {
                Console.Write("Enter command:");
                string commandLine = Console.ReadLine();
                gameIsRunning = commandProcessor.ProcessCommand(commandLine);
            }
        }
    }
}
