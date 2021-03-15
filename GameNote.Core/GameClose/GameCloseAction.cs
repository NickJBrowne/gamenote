using System;

namespace GameNote.Core.GameClose
{
    public class GameCloseAction
    {
        public static GameCloseAction OpenUrl(Uri url)
            => new GameCloseAction(GameCloseActionEnum.OpenUrl, url.ToString());

        public static GameCloseAction DoNothing()
            => new GameCloseAction(GameCloseActionEnum.DoNothing, string.Empty);

        public static GameCloseAction OpenProgram(string path)
            => new GameCloseAction(GameCloseActionEnum.OpenProgram, path);

        public GameCloseAction(GameCloseActionEnum action, string arguments)
        {
            Action = action;
            Arguments = arguments;
        }

        public GameCloseActionEnum Action { get; set; }
        public string Arguments { get; set; }

        public bool ShowAskDialog()
            => this.Action != GameCloseActionEnum.DoNothing;        
    }
}