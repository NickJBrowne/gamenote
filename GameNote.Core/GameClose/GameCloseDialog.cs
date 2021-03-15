namespace GameNote.Core.GameClose
{
    public class GameCloseDialog
    {
        private readonly IDialogHandler _dialogHandler;

        public GameCloseDialog(IDialogHandler dialogHandler)
        {
            _dialogHandler = dialogHandler;
        }

        public GameCloseAction Ask(GameCloseAction onSuccessAction)
        {
            if (onSuccessAction == null)
                throw new System.ArgumentNullException("OnSuccess value is not defined");

            if (onSuccessAction.ShowAskDialog() == false)
                return onSuccessAction;

            var result = _dialogHandler.AskYesNo($"Do you want to take notes for this game?");
            if (result == DialogResult.Yes)
                return onSuccessAction;
            else
                return GameCloseAction.DoNothing();
        }
    }
}