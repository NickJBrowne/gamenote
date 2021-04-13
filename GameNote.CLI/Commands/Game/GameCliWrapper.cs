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

        public async Task<CliCommandResult> Run(string game, bool force=false)
        {
            var args = new CliBuilder(_command)
                .Option(RunCommand.GameKey, game)
                .Option(RunCommand.ForceKey, force)
                .Build();

            return await Run(args);
        }
    }
}
