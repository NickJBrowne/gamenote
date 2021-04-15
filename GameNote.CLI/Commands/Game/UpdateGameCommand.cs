using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = CommandKeys.Game_Update, Description = "Update a game")]
    public class UpdateGameCommand : BaseCommand
    {
        private readonly GameSettingBuilder _gameSettingsBuilder;
        private readonly ISettingsHandler _settingsHandler;

        public UpdateGameCommand(GameSettingBuilder builder, ISettingsHandler settingsHandler)
        {
            _gameSettingsBuilder = builder;
            _settingsHandler = settingsHandler;
        }

        [Option(
            CommandOptionType.SingleValue,
            ShortName = "g",
            LongName = "game",
            Description = "The game to update"
        )]
        public string Game { get; set; }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "u", 
            LongName = "url", 
            Description = "The url to open when the game closes"
        )]
        public string OpenUrl { get; set; }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "c", 
            LongName = "cmd", 
            Description = "The command to run through command prompt"
        )]
        public string Command { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            var settings = _settingsHandler.Load();

            if (Game.HasValue() == false)
                return Fail("No game specified");

            var game = settings.FindGame(Game);
            if (game == null)
                return Fail($"Could not find a game by name {game}");

            _gameSettingsBuilder.Load(game);
            string action = "";

            if (OpenUrl.HasValue())
            {
                _gameSettingsBuilder.OnGameCloseOpenUrl(OpenUrl);
                action = $"Open url -> {OpenUrl}";
            }
            else if (Command.HasValue())
            {
                _gameSettingsBuilder.OnGameCloseOpenProgram(Command);
                action = $"Run Command -> {Command}";
            }
            else
            {
                _gameSettingsBuilder.OnGameCloseDoNothing();
                action = "Do nothing";
            }

            string fileName = _gameSettingsBuilder.Build();
            return Success($"Updated game: {fileName} to {action}");
        }
    }
}