using System.IO;
using Newtonsoft.Json;

namespace GameNote.Core
{
    public class SettingsHandler : ISettingsHandler
    {
        private readonly string _directory;
        private readonly string _settingsFilePath;
        private static Settings _settings;
        private static string _settingName = "settings.json";

        public SettingsHandler(string directory)
        {
            _directory = directory;
            _settingsFilePath = Path.Combine(directory, _settingName);

        }
        public Settings Load()
        {
            if (_settings == null)
            {
                if (File.Exists(_settingsFilePath))
                    _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_settingsFilePath));
                else
                    _settings = Save(new Settings());
            }

            return _settings;
        }

        public Settings Save(Settings settings)
        {
            _settings = settings;
            
            if (File.Exists(_settingsFilePath))
                File.Delete(_settingsFilePath);

            if (Directory.Exists(_directory) == false)
                Directory.CreateDirectory(_directory);

            File.WriteAllText(_settingsFilePath, JsonConvert.SerializeObject(_settings, Formatting.Indented));
            return _settings;
        }
    }
}