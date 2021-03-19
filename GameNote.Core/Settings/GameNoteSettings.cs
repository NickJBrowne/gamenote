using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameNote.Core.GameClose;

namespace GameNote.Core.Settings
{
    public class GameNoteSettings
    {
        public List<GameSetting> Games { get; set; } = new List<GameSetting>();
        public string PathToCLI { get; set; }

        public GameNoteSettings(string pathToCLI)
        {
            PathToCLI = pathToCLI;
        }

        public void AddGame(string filePath, GameCloseAction closeAction)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new System.Exception($"You must specify a file path");

            if (closeAction == null)
                throw new System.ArgumentNullException("Close action must be specified");

            Games.Add(new GameSetting(filePath, closeAction));
        }

        public GameSetting FindGame(string fileName)
            => Games.SingleOrDefault(g => g.FileName.ToLower() == fileName.ToLower());

        public bool HasGames()
            => this.Games.Any();

        public bool HasSettings(FileInfo fileInfo)
            => this.Games.Any(g => g.FilePath.ToLower() == fileInfo.FullName.ToLower());

        public CLIHandler GetCLI()
            => new CLIHandler(PathToCLI);
    }
}