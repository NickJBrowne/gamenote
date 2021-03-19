using GameNote.Core.GameClose;

namespace GameNote.Core
{
    public class GameSetting 
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public GameCloseAction GameCloseAction { get; set; }

        public GameSetting(string filePath, GameCloseAction closeAction)
        {
            FileName = System.IO.Path.GetFileName(filePath);
            FilePath = filePath;
            GameCloseAction = closeAction;
        }
    }
}