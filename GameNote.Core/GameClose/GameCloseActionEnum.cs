using System.ComponentModel;

namespace GameNote.Core.GameClose
{
    public enum GameCloseActionEnum
    {
        [Description("Do nothing")]
        DoNothing = 0,

        [Description("Open Url")]
        OpenUrl = 1,

        [Description("Open program")]
        OpenProgram = 2
    }
}