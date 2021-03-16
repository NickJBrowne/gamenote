using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameNote.Core.GameList;

namespace GameNote.CLI.Interfaces
{
    public class FileSystemHandler : IFileSystemHandler
    {
        public bool DoesDirectoryExist(string path)
            => File.Exists(path);

        public IEnumerable<FileInfo> GetExecutableFiles(string path)
            => Directory.GetFiles(
                path,
                "*.exe",
                SearchOption.AllDirectories
            ).Select(file => new FileInfo(file));
    }
}