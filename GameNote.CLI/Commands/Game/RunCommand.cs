using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.GameClose;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = CommandKeys.Game_Run, Description = "Run the on close action for a game")]
    internal class RunCommand : BaseCommand
    {
        public const string ForceKey = "force";
        public const string GameKey = "game";

        private readonly IDialogHandler _dialogHandler;
        private readonly IGameCloseActionHandler _gameCloseActionHandler;
        private readonly ISettingsHandler _settingsHandler;

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "f", 
            LongName = ForceKey, 
            Description = "Force through the action without checking for the users input"
        )]
        public bool Force { get; set; } = false;

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "g", 
            LongName = GameKey, 
            Description = "The game whose on close you want to run"
        )]
        public string Game { get; set; }

        public RunCommand(
            ISettingsHandler settingsHandler, 
            IGameCloseActionHandler gameCloseActionHandler, 
            IDialogHandler dialogHandler
        )
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

            GameSetting gameToRun = settings.FindGame(Game);
            if (gameToRun == null)
                return Fail($"No game found for: {Game}");

            if (Force == true)
            {
                Message("Forcing game close event");
                RunGameCloseEvent(gameToRun);
                return Success();
            }

            var dialogResult = _dialogHandler.AskYesNo($"Do you want to take notes for the game {gameToRun.FileName}?");
            if (dialogResult == DialogResult.Yes)
            {
                Message("User selected to run the action");
                RunGameCloseEvent(gameToRun);
            }
            else
                Message("User selected to not run the action");
        
            return Success();
        }

        private void RunGameCloseEvent(GameSetting gameToRun)
        {
            _gameCloseActionHandler.Run(
                gameToRun,
                (message) => Message(message)
            );
        }
    }
}