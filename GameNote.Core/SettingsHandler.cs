using System;
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
        private static DateTime? _lastWritten;
        private static IFileSystemHandler _fileSystemHandler;

        public SettingsHandler(string directory, IFileSystemHandler fileSystemHandler)
        {
            _directory = directory;
            _settingsFilePath = Path.Combine(directory, _settingName);
            _fileSystemHandler = fileSystemHandler;
        }

        public Settings Load()
        {
            if (_settings == null)
            {
                if (_fileSystemHandler.DoesFileExist(_settingsFilePath))
                {
                    string fileData = _fileSystemHandler.GetFileData(_settingsFilePath);
                    _settings = JsonConvert.DeserializeObject<Settings>(fileData);
                    _lastWritten = _fileSystemHandler.GetLastWriteTime(_settingsFilePath);
                }
                else
                    _settings = Save(new Settings());
            }

            return _settings;
        }

        public Settings Save(Settings settings)
        {
            _settings = settings;

            _fileSystemHandler.CreateDirectoryIfDoesntExist(_directory);
            _fileSystemHandler.DoesFileExist(_settingsFilePath);
            _fileSystemHandler.WriteFileData(_settingsFilePath, _settings);

            return _settings;
        }

        public bool HasChangedSinceLastLoad()
        {
            if (_fileSystemHandler.DoesFileExist(_settingsFilePath) == false)
                return false;

            DateTime lastWritten = _fileSystemHandler.GetLastWriteTime(_settingsFilePath);

            if (_lastWritten == null)
                return true;

            return lastWritten > _lastWritten.Value;   
        }
    }
}