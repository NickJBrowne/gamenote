using System;
using System.Windows.Forms;
using GameNote.Core;
using GameNote.Core.Settings;
using GameNote.WinConfirm;

namespace GameNote.CLI
{
    public class DialogHandler : IDialogHandler
    {
        public GameNote.Core.DialogResult AskYesNo(string question)
        {
            Application.EnableVisualStyles();
            var dialog = new CheckDialog(question);
            Application.Run(dialog);

            return GameNote.Core.DialogResult.Yes;
        }
    }
}