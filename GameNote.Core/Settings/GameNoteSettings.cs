using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameNote.Core.GameClose;

namespace GameNote.Core.Settings
{
    public class GameNoteSettings
    {
        public List<GameSetting> Games { get; set; } = new List<GameSetting>();

        public void AddGame(string filePath, GameCloseAction closeAction)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new System.Exception($"You must specify a file path");

            if (closeAction == null)
                throw new System.ArgumentNullException("Close action must be specified");

            var gameSettings = new GameSetting(filePath, closeAction);

            if (Games.Any(g => g.FilePath == filePath))
            {
                var targetGame = Games.First(g => g.FilePath == filePath);
                targetGame.FilePath = gameSettings.FilePath;
                targetGame.FileName = gameSettings.FileName;
                targetGame.GameCloseAction = gameSettings.GameCloseAction;                
            }
            else
                Games.Add(gameSettings);
        }

        public GameSetting FindGame(string fileName)
            => Games.SingleOrDefault(g => g.FileName.ToLower().Contains(fileName.ToLower()));

        public bool HasGames()
            => this.Games.Any();

        public bool HasSettings(FileInfo fileInfo)
            => this.Games.Any(g => g.FilePath.ToLower() == fileInfo.FullName.ToLower());

        public bool Remove(string gameName)
            => Games.Remove(r => r.FileName.ToLower().Contains(gameName.ToLower()));
    }
}