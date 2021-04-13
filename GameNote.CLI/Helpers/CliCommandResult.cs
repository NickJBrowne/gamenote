using System;

namespace GameNote.CLI.Helpers
{
    public class CliCommandResult
    {
        public bool IsSuccess { get; set; }

        public CliCommandResult(bool success)
        {
            IsSuccess = success;
        }

        public static CliCommandResult Success() => new CliCommandResult(true);
        public static CliCommandResult Fail() => new CliCommandResult(false);
    }
}
