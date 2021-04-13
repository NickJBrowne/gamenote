using System.Threading.Tasks;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = CommandKeys.Game_Remove, Description = "Remove a game")]
    class RemoveGameCommand : BaseCommand
    {
        private readonly ISettingsHandler _settingsHandler;

        public RemoveGameCommand(ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
        }

        [Option(
            CommandOptionType.SingleValue, 
            ShortName = "g", 
            LongName = "game", 
            Description = "The game whose on close you want to run"
        )]
        public string Game { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            var settings = _settingsHandler.Load();

            if (settings.Remove(Game))
            {
                _settingsHandler.Save(settings);
                return Success("Game removed");
            }
            else
                return Fail($"Could not find game by name {Game} to remove");
        }
    }
}