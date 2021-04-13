using System;
using System.Collections.Generic;

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

        public string[] Build()
            => _args.ToArray();
    }
}
