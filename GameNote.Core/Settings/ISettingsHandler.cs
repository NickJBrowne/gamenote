namespace GameNote.Core.Settings
{
    public interface ISettingsHandler
    {
        GameNoteSettings Load();
        GameNoteSettings Save(GameNoteSettings settings);
        bool HasChangedSinceLastLoad();
        string GetPathToSettingsFile();
    }
}