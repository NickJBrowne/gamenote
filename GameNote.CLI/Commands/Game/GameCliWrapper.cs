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
            => await Command<AddGameCommand>()
                .Option(c => c.DirectPath, fullPathToGame)
                .Option(c => c.OpenUrl, urlToOpen.ToString())
                .Run();

        public async Task<CliCommandResult> Run(string game, bool force=false)
            => await Command<RunCommand>()
                .Option(c => c.Game, game)
                .Option(c => c.Force, force)
                .Run();
    }
}
