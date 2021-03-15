namespace GameNote.Core
{
    public interface IDialogHandler
    {
        DialogResult AskYesNo(string question);
    }

    public enum DialogResult
    {
        Yes,
        No
    }
}