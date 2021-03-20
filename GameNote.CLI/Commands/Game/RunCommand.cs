using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.GameClose;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = "run", Description = "Run the on close action for a game")]
    internal class RunCommand : BaseCommand
    {
        private readonly GameCloseDialog _dialogHandler;
        private readonly IGameCloseActionHandler _gameCloseActionHandler;
        private readonly ISettingsHandler _settingsHandler;

        [Option(CommandOptionType.SingleValue, ShortName = "f", LongName = "force", Description = "Force through the action without checking for the users input")]
        public bool Force { get; set; } = false;

        [Option(CommandOptionType.SingleValue, ShortName = "g", LongName = "game", Description = "The game whose on close you want to run")]
        public string Game { get; set; }

        public RunCommand(ISettingsHandler settingsHandler, IGameCloseActionHandler gameCloseActionHandler, GameCloseDialog dialogHandler)
        {
            _settingsHandler = settingsHandler;
            _gameCloseActionHandler = gameCloseActionHandler;
            _dialogHandler = dialogHandler;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            if (Game.HasValue() == false)
                return Fail("You must provide a game to run");

            var settings = _settingsHandler.Load();

            var gameToRun = settings.FindGame(Game);
            if (gameToRun == null)
                return Fail($"No game found for: {Game}");

            if (Force == false)
            {
                var action = _dialogHandler.Ask(gameToRun.GameCloseAction);
                _gameCloseActionHandler.Run(
                    new GameSetting(gameToRun.FilePath, action),
                    (message) => Message(message)
                );
            }
            else
                _gameCloseActionHandler.Run(
                    gameToRun,
                    (message) => Message(message)
                );

            return Success();
        }
    }
}