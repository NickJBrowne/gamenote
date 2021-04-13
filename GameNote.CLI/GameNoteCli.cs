using System;
using GameNote.CLI.Commands.Game;

namespace GameNote.CLI
{
    public class GameNoteCli
    {
        public GameCliWrapper Game { get; set; }

        public GameNoteCli()
        {
            Game = new GameCliWrapper();
        }
    }
}
