using System;
using GameNote.Core;

namespace GameNote.CLI
{
    public class DialogHandler : IDialogHandler
    {
        public GameNote.Core.DialogResult AskYesNo(string question)
        {
            Console.WriteLine("Ask something");
            return GameNote.Core.DialogResult.Yes;
        }
    }
}