using System.Collections.Generic;
using System.IO;

namespace GameNote.Core
{
    public interface IFileSystemHandler
    {
        bool DoesDirectoryExist(string path);

        bool DoesFileExist(string path);

        IEnumerable<FileInfo> GetExecutableFiles(string path);
    }
}