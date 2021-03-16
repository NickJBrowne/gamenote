using System.Collections.Generic;
using System.IO;

namespace GameNote.Core.GameList
{
    public interface IFileSystemHandler
    {
        bool DoesDirectoryExist(string path);

        IEnumerable<FileInfo> GetExecutableFiles(string path);
    }
}