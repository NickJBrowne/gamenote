using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = CommandKeys.Game_AddGame, Description = "Add a game")]
    class AddGameCommand : BaseCommand
    {
        public const string FullPathKey = "full-path";
        public const string DirectoryKey = "directory";
        public const string GameKey = "game";
        public const string UrlKey = "url";
        public const string CommandKey = "cmd";

        private readonly GameSettingBuilder _gameSettingsBuilder;

        public AddGameCommand(GameSettingBuilder gameSettingsBuilder)
        {
            _gameSettingsBuilder = gameSettingsBuilder;
        }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "p", 
            LongName = FullPathKey, 
            Description = "The full path to the game"
        )]
        public string DirectPath { get; set; }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "d", 
            LongName = DirectoryKey, 
            Description = "A directory to look under, use with -exe|--file-name"
        )]
        public string Directory { get; set; }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "g", 
            LongName = GameKey, 
            Description = "The name of the file to look for under the directory, use with -d|--directory"
        )]
        public string FileName { get; set; }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "u", 
            LongName = UrlKey, 
            Description = "The url to open when the game closes"
        )]
        public string OpenUrl { get; set; }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "c", 
            LongName = CommandKey, 
            Description = "The command to run through command prompt"
        )]
        public string Command { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            if (DirectPath.HasValue())
                _gameSettingsBuilder.FromFullPath(DirectPath);
            else if (Directory.HasValue() && FileName.HasValue())
                _gameSettingsBuilder.WithinDirectory(Directory, FileName);
            else
                return Fail("You must specify either a direct file path or a folder path and a file name");

            if (OpenUrl.HasValue())
                _gameSettingsBuilder.OnGameCloseOpenUrl(OpenUrl);
            else if (Command.HasValue())
                _gameSettingsBuilder.OnGameCloseOpenProgram(Command);
            else
            {
                Message("On game close, nothing will happen");
                _gameSettingsBuilder.OnGameCloseDoNothing();
            }

            string fileName = _gameSettingsBuilder.Build();
            SuccessMessage($"Added game: {fileName}");
            return Success();
        }
    }
}