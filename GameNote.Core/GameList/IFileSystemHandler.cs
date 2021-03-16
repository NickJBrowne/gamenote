using System.Collections.Generic;
using System.IO;

namespace GameNote.Core.GameList
{
    public interface IFileSystemHandler
    {
        bool DoesDirectoryExist(string path);

        List<FileInfo> GetExecutableFiles(string path);
    }
}