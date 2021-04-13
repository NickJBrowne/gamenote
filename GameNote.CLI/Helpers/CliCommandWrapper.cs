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

        protected CliBuilder Command() => new CliBuilder(_command);
    }
}
