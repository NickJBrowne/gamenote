using System.Threading.Tasks;
using GameNote.CLI.Commands.Game;
using GameNote.CLI.Commands.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands
{
    [Command(Name = "gamenote")]
    [Subcommand(
        typeof(BaseGameCommand),
        typeof(BaseSettingsCommand)
    )]
    class GameNoteCommand : BaseCommand
    {
        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();            
            return Success();
        }
    }
}