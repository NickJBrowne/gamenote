using System;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameNote.Core.Settings
{
    public class SettingsHandler : ISettingsHandler
    {
        private readonly string _directory;
        private readonly string _settingsFilePath;
        private static GameNoteSettings _settings;
        private static string _settingName = "settings.json";
        private static DateTime? _lastWritten;
        private static IFileSystemHandler _fileSystemHandler;
        private readonly string _pathToCLI = string.Empty;

        public SettingsHandler(string directory, IFileSystemHandler fileSystemHandler, IOptions<Configuration> optionsConfiguration)
        {
            _directory = directory;
            _settingsFilePath = Path.Combine(directory, _settingName);
            _fileSystemHandler = fileSystemHandler;
            _pathToCLI = optionsConfiguration.Value.PathToCLI;
        }

        public GameNoteSettings Load()
        {
            if (_settings == null)
            {
                if (_fileSystemHandler.DoesFileExist(_settingsFilePath))
                {
                    string fileData = _fileSystemHandler.GetFileData(_settingsFilePath);
                    _settings = JsonConvert.DeserializeObject<GameNoteSettings>(fileData);
                    _lastWritten = _fileSystemHandler.GetLastWriteTime(_settingsFilePath);
                }
                else
                {
                    if (new CLIHandler(_pathToCLI).IsValidPath() == false)
                        throw new Exception("Cannot create new settings because path to cli is missing");

                    _settings = Save(new GameNoteSettings(_pathToCLI));
                }
            }

            return _settings;
        }

        public GameNoteSettings Save(GameNoteSettings settings)
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

        public string GetPathToSettingsFile()
            => _settingsFilePath;
    }
}