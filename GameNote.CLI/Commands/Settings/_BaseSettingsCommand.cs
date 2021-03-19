using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Settings
{
    [Command(Name = "settings", Description = "Settings options")]
    [Subcommand(
        typeof(OpenFolderCommand),
        typeof(ResetCommand)
    )]
    class BaseSettingsCommand : BaseCommand
    {
        
    }
}