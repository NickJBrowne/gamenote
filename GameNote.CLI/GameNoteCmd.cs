using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI
{
    [Command(Name = "GameNote CLI", OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase )]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(Commands.GameCmd)
    )]
    public class GameNoteCmd
    {
        private static string GetVersion()
            => typeof(GameNoteCmd).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}