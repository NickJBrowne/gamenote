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

        public IEnumerable<Game> Run(string directory, bool returnBlacklisted=false)
        {
            if (_fileSystem.DoesDirectoryExist(directory) == false)
                throw new System.Exception($"Could not find directory: {directory}");

            var files = _fileSystem.GetExecutableFiles(directory);
            if (files.Any() == false)
                throw new System.Exception($"No files found within directory {directory}");

            var blacklist = new ExecutableBlacklist();            

            var result = new List<Game>();
            foreach(var file in files)
            {
                bool isAlreadyConfigured = _settings.HasSettings(file);
                bool isBlacklisted = blacklist.IsInBlackList(file);

                if (isAlreadyConfigured == true)
                    isBlacklisted = false;

                var item = new Game()
                {
                    Executable = file.Name,
                    FullPath = file.FullName,
                    AlreadyConfigured = isAlreadyConfigured,
                    IsBlacklisted = isBlacklisted
                };
                result.Add(item);
            }

            if (returnBlacklisted == false)
                return result.Where(r => r.IsBlacklisted == false);

            return result;
        }
    }
}