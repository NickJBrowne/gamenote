using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameNote.Core.GameList
{
    public class GetGamesInDirectory
    {
        private IFileSystemHandler _fileSystem;
        private Settings _settings;

        public GetGamesInDirectory(IFileSystemHandler fileSystem, Settings settings)
        {
            _fileSystem = fileSystem;
            _settings = settings;
        }

        public List<Game> Run(string directory)
        {
            if (_fileSystem.DoesDirectoryExist(directory) == false)
                throw new System.Exception($"Could not find directory: {directory}");

            var rawFiles = _fileSystem.GetExecutableFiles(directory);
            var files = RunAgainstBlacklist(rawFiles);
            if (files.Any() == false)
                throw new System.Exception($"No files found within directory {directory}");

            var result = new List<Game>();
            foreach(var file in files)
            {
                var item = new Game()
                {
                    Executable = file.Name,
                    FullPath = file.FullName,
                    AlreadyConfigured = _settings.HasSettings(file)
                };
                result.Add(item);
            }
            return result;
        }

        private List<FileInfo> RunAgainstBlacklist(List<FileInfo> files)
        {
            var result = new List<FileInfo>();

            if (files.Any() == false)
                return result;

            var blacklist = new ExecutableBlacklist();

            foreach (var file in files)
            {
                if (blacklist.IsInBlackList(file) == false)
                    result.Add(file);
            }

            return result;
        }
    }
}