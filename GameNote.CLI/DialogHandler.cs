using System;
using System.Windows.Forms;
using GameNote.Core;
using GameNote.Core.Settings;
using GameNote.WindowsConfirm;

namespace GameNote.CLI
{
    public class DialogHandler : IDialogHandler
    {
        public GameNote.Core.DialogResult AskYesNo(string question)
        {
            Application.EnableVisualStyles();
            var dialog = new ConfirmDialog(question);
            Application.Run(dialog);

            if (dialog.SelectedYes == true)
                return GameNote.Core.DialogResult.Yes;
            else
                return GameNote.Core.DialogResult.No;
        }
    }
}