using System;
using System.Threading.Tasks;

namespace GameNote.CLI.Helpers
{
    public abstract class CliCommandWrapper
    {
        protected string _command;

        public CliCommandWrapper(string command)
        {
            _command = command;
        }

        public async Task<CliCommandResult> Run(string[] args)
        {
            var result = await Program.Main(args);
            return result == 1 ? CliCommandResult.Fail() : CliCommandResult.Success();
        }
    }
}
