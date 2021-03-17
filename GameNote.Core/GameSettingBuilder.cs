using System;
using System.IO;
using System.Linq;
using GameNote.Core.GameClose;

namespace GameNote.Core
{
    public class GameSettingBuilder
    {
        private readonly IFileSystemHandler _fileSystemHandler;
        private readonly ISettingsHandler _settingsHandler;

        private FileInfo _filePath { get; set; }
        private GameCloseActionEnum? _action { get; set; }
        private string _arguments { get; set; }
        
        public GameSettingBuilder(IFileSystemHandler fileSystemHandler, ISettingsHandler settingsHandler)
        {
            _fileSystemHandler = fileSystemHandler;
            _settingsHandler = settingsHandler;
        }

        public GameSettingBuilder FromFullPath(string fullPath)
        {
            if (_fileSystemHandler.DoesFileExist(fullPath))
                throw new System.Exception($"File does not exist {fullPath}");

            var fileInfo = new FileInfo(fullPath);
            if (fileInfo.Extension != ".exe")
                throw new System.Exception($"File: {fullPath} is not an executable file");

            _filePath = fileInfo;
            return this;
        }

        public GameSettingBuilder WithinDirectory(string folder, string fileName)
        {
            var blackList = new ExecutableBlacklist();

            var files = _fileSystemHandler
                .GetExecutableFiles(folder)
                .Where(file => blackList.IsInBlackList(file) == false)
                .Where(file => file.Name == fileName);

            if (files.Count() == 0)
                throw new System.Exception("No file found that match this criteria");
            else if (files.Count() == 1)
                _filePath = files.First();
            else
                throw new System.Exception("There are too many files that match this criteria");

            return this;
        }

        public GameSettingBuilder OnGameCloseOpenUrl(string url)
        {
            var uri = new Uri(url);
            _action = GameCloseActionEnum.OpenUrl;
            _arguments = uri.ToString();
            return this;
        }

        public GameSettingBuilder OnGameCloseOpenProgram(string arguments)
        {
            _action = GameCloseActionEnum.OpenProgram;
            _arguments = arguments;
            return this;
        }

        public GameSettingBuilder OnGameCloseDoNothing()
        {
            _action = GameCloseActionEnum.DoNothing;
            _arguments = string.Empty;
            return this;
        }

        public string Build()
        {
            if (_filePath == null)
                throw new Exception("FilePath has not been provided");

            if (_action == null)
                throw new Exception("OnCloseAction is not provided");

            if (string.IsNullOrEmpty(_arguments))
                throw new Exception("OnCloseAction arguments is not provided");

            var settings = _settingsHandler.Load();
            settings.AddGame(_filePath.FullName, new GameCloseAction(_action.Value, _arguments));
            _settingsHandler.Save(settings);

            return _filePath.Name;
        }
    }
}