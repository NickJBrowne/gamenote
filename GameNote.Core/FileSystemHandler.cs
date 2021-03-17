using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameNote.Core
{
    public class FileSystemHandler : IFileSystemHandler
    {
        public bool DoesDirectoryExist(string path)
            => Directory.Exists(path);

        public bool DoesFileExist(string path)
            => File.Exists(path);

        public IEnumerable<FileInfo> GetExecutableFiles(string path)
            => Directory.GetFiles(
                path,
                "*.exe",
                SearchOption.AllDirectories
            ).Select(file => new FileInfo(file));
    }
}