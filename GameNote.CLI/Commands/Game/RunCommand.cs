using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.GameClose;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = "run", Description = "Run the on close action for a game")]
    class RunCommand : BaseCommand
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly GameCloseDialog _dialogHandler;
        private readonly IGameCloseActionHandler _gameCloseActionHandler;

        public RunCommand(ISettingsHandler settingsHandler, IGameCloseActionHandler gameCloseActionHandler, GameCloseDialog dialogHandler)
        {
            _settingsHandler = settingsHandler;
            _gameCloseActionHandler = gameCloseActionHandler;
            _dialogHandler = dialogHandler;
        }

        [Option(CommandOptionType.SingleValue, ShortName = "g", LongName = "game", Description = "The game whose on close you want to run")]
        public string Game { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            if (Game.HasValue() == false)
                return Fail("You must provide a game to run");

            var settings = _settingsHandler.Load();

            var gameToRun = settings.FindGame(Game);
            if (gameToRun == null)
                return Fail($"No game found for: {Game}");  

            var action = _dialogHandler.Ask(gameToRun.GameCloseAction);
            _gameCloseActionHandler.Run(
                new GameSetting(gameToRun.FilePath, action), 
                (message) => Message(message)
            );

            return Success();
        }
    }
}