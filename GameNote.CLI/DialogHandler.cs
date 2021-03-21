using System;
using System.Windows.Forms;
using GameNote.Core;
using GameNote.Core.Settings;
using GameNote.WinConfirm;

namespace GameNote.CLI
{
    public class DialogHandler : IDialogHandler
    {
        private readonly ISettingsHandler _settingsHandler;

        public DialogHandler(ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
        }

        public GameNote.Core.DialogResult AskYesNo(string question)
        {
            /*var settings = _settingsHandler.Load();

            Application.EnableVisualStyles();
            var dialog = new CheckDialog(question);
            Application.Run(dialog);

            dialog.
            */
            return GameNote.Core.DialogResult.Yes;
        }
    }
}