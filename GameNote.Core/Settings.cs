using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameNote.Core.GameClose;

namespace GameNote.Core
{
    public class Settings
    {
        public List<GameSetting> Games { get; set; } = new List<GameSetting>();

        public void AddGame(string filePath, GameCloseAction closeAction)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new System.Exception($"You must specify a file path");

            if (closeAction == null)
                throw new System.ArgumentNullException("Close action must be specified");

            Games.Add(new GameSetting()
            {
                FilePath = filePath,
                GameCloseAction = closeAction
            });
        }

        public bool HasGames()
            => this.Games.Any();

        public bool HasSettings(FileInfo fileInfo)
            => this.Games.Any(g => g.FilePath == fileInfo.FullName);

        public class GameSetting 
        {
            public string FilePath { get; set; }
            public GameCloseAction GameCloseAction { get; set; }
        }
    }
}