using System;
using System.Threading.Tasks;
using GameNote.CLI.Commands;

namespace GameNote.CLI.Helpers
{
    public abstract class CliCommandWrapper
    {
        protected string _command;

        public CliCommandWrapper(string command)
        {
            _command = command;
        }

        internal CliBuilder Command() => new CliBuilder(_command);
        internal CliBuilder<T> Command<T>()
            where T : BaseCommand
            => new CliBuilder<T>(_command);
    }
}
