using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameNote.CLI.Helpers
{
    public class CliBuilder
    {
        private List<string> _args = new List<string>();

        public CliBuilder(string start)
        {
            _args.Add(start);
        }

        public CliBuilder Option(string name, string value)
        {
            _args.Add($"--{name}");
            _args.Add(value);

            return this;
        }

        public CliBuilder Option(string name, bool value)
        {
            if (value == true)
                _args.Add($"--{name}");

            return this;
        }

        public async Task<CliCommandResult> Run()
        {
            var result = await Program.Main(_args.ToArray());
            return result == 1 ? CliCommandResult.Fail() : CliCommandResult.Success();
        }
    }
}
