using System.IO;
using System.Threading.Tasks;
using GameNote.CLI.Helpers;
using GameNote.Core.GameList;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = CommandKeys.Game_Find, Description = "Find game files within a directory")]
    class FindCommand : BaseCommand
    {
        private readonly GetGamesInDirectory _handler;
        private readonly ISettingsHandler _settingsHandler;

        public FindCommand(GetGamesInDirectory handler, ISettingsHandler settingsHandler)
        {
            _handler = handler;
            _settingsHandler = settingsHandler;
        }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = CliCommandKeys.Directory.Short, 
            LongName = CliCommandKeys.Directory.Long, 
            Description = "The directory to look in"
        )]
        public string Directory { get; set; }

        [Option(
            CommandOptionType.SingleOrNoValue,
            ShortName = "b",
            LongName = "show-blacklisted",
            Description = "Show blacklisted items"
        )]
        public bool ShowBlacklisted { get; set; } = false;

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            Message($"Looking for games under {Directory}");
            
            var settings = _settingsHandler.Load();
            var result = _handler.Run(Directory, ShowBlacklisted);
            
            ConsoleWriteTable.New(result)
                .AddColumn("File Name", x => x.Executable)
                .AddColumn("Full Path", x => x.FullPath.Replace(Directory, ""), ColumnAlign.Left)
                .AddColour(
                    g => settings.FindGame(Path.GetFileName(g.FullPath)) != null,
                    System.ConsoleColor.Green
                )
                .AddColour(
                    g => g.IsBlacklisted,
                    System.ConsoleColor.Red
                )
                .Write();

            return Success();
        }
    }
}