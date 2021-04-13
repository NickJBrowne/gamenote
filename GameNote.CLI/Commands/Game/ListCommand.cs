using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameNote.CLI.Helpers;
using GameNote.Core;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = CommandKeys.Game_List, Description = "List all configured games")]
    class ListCommand : BaseCommand
    {
        private readonly ISettingsHandler _settingsHandler;

        public ListCommand(ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            var settings = _settingsHandler.Load();

            if (settings.Games.Any() == false)
                return Fail("No games found");

            ConsoleWriteTable.New(settings.Games)
                .AddColumn("File Name", g => Path.GetFileName(g.FilePath))
                .AddColumn("File Path", g => g.FilePath, ColumnAlign.Left)
                .AddColumn("OnCloseAction", g => g.GameCloseAction.Action.GetDescription())
                .AddColumn("Arguments", g => g.GameCloseAction.Arguments)
                .Write();
                
            return Success();
        }
    }
}