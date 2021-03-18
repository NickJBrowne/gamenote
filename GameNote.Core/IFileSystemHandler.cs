using System;
using System.Collections.Generic;
using System.IO;

namespace GameNote.Core
{
    public interface IFileSystemHandler
    {
        bool DoesDirectoryExist(string path);
        bool DoesFileExist(string path);
        IEnumerable<FileInfo> GetExecutableFiles(string path);
        void DeleteFileIfExists(string path);
        DateTime GetLastWriteTime(string path);
        void CreateDirectoryIfDoesntExist(string directory);
        string GetFileData(string path);
        void WriteFileData(string path, object fileData);
    }
}