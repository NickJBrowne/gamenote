namespace GameNote.Core
{
    public interface ISettingsHandler
    {
        Settings Load();
        Settings Save(Settings settings);
    }
}