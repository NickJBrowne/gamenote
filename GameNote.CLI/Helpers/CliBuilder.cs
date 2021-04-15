using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GameNote.CLI.Commands;
using GameNote.Core;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Helpers
{
    public class CliBuilder<T> : CliBuilder
        where T : BaseCommand
    {
        public CliBuilder(string start) : base(start)
        {
        }

        public CliBuilder<T> Option(Expression<Func<T, string>> selector, string value)
        {
            string name = GetCommandName(selector);
            Option(name, value);

            return this;
        }

        public CliBuilder<T> Option(Expression<Func<T, bool>> selector, bool value)
        {
            string name = GetCommandName(selector);
            Option(name, value);

            return this;
        }

        private string GetCommandName<TOut>(Expression<Func<T, TOut>> selector)
        {
            var prop = (PropertyInfo)((MemberExpression)selector.Body).Member;
            var attributes = prop.GetCustomAttributes<OptionAttribute>();

            if (attributes.Any() == false)
                throw new Exception($"Could not find OptionAttribute for {prop.Name}");

            foreach (var attr in attributes)
            {
                if (attr.LongName != null && attr.LongName.HasValue())
                    return attr.LongName;

                if (attr.ShortName != null && attr.ShortName.HasValue())
                    return attr.ShortName;
            }

            throw new Exception($"Could not find option Long or Short name for {prop.Name}");
        }
    }

    public class CliBuilder
    {
        private List<string> _args = new List<string>();

        public CliBuilder(string start)
        {
            _args.Add(start);
        }

        public CliBuilder Option(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
                return this;

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

        public string[] GetArguments()
            => _args.ToArray();

        public async Task<CliCommandResult> Run()
        {
            var result = await Program.Main(GetArguments());
            return result == 1 ? CliCommandResult.Fail() : CliCommandResult.Success();
        }
    }
}
