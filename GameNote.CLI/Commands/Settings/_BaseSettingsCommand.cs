using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Settings
{
    [Command(Name = CommandKeys.Settings, Description = "Settings options")]
    [Subcommand(
        typeof(OpenFolderCommand),
        typeof(ResetCommand)
    )]
    public class BaseSettingsCommand : BaseCommand
    {
        
    }
}