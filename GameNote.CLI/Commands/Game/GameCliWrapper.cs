using System;
using System.Threading.Tasks;
using GameNote.CLI.Helpers;

namespace GameNote.CLI.Commands.Game
{
    public class GameCliWrapper : CliCommandWrapper
    {
        public GameCliWrapper() : base(CommandKeys.Game)
        {
        }

        public async Task<CliCommandResult> Add(string fullPathToGame, Uri urlToOpen)
            => await Command()
                .Option(AddGameCommand.FullPathKey, fullPathToGame)
                .Option(AddGameCommand.UrlKey, urlToOpen.ToString())
                .Run();

        public async Task<CliCommandResult> Run(string game, bool force=false)
            => await Command()
                .Option(RunCommand.GameKey, game)
                .Option(RunCommand.ForceKey, force)
                .Run();
    }
}
