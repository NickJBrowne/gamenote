using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands
{
    [HelpOption]
    abstract class BaseCommand
    {
        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {            
            return Success();
        }

        public Task<int> Success()
            => Task.FromResult(0);

        public Task<int> Success(string message)
        {
            SuccessMessage(message);
            return Success();
        }

        public Task<int> Fail()
            => Task.FromResult(1);

        public Task<int> Fail(string message)
        {
            ErrorMessage(message);
            return Fail();
        }

        public void Message(string message)
            => Console.WriteLine(message);

        public void ErrorMessage(string message)
            => SwapColourForMessage(ConsoleColor.Red, message);

        public void SuccessMessage(string message)
            => SwapColourForMessage(ConsoleColor.Green, message);

        private void SwapColourForMessage(ConsoleColor colour, string message)
        {
            var startingColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Message(message);
            Console.ForegroundColor = startingColour;
        }
    }
}