using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GameNote.Core
{
    public class FileSystemHandler : IFileSystemHandler
    {
        public void CreateDirectoryIfDoesntExist(string directory)
        {
            if (this.DoesDirectoryExist(directory) == false)
                Directory.CreateDirectory(directory);   
        }

        public void DeleteFileIfExists(string path)
        {
            if (this.DoesFileExist(path) == false)
                File.Delete(path);
        }

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

        public string GetFileData(string path)
            => File.ReadAllText(path);

        public DateTime GetLastWriteTime(string path)
            => File.GetLastWriteTimeUtc(path);

        public void WriteFileData(string path, object fileData)
        {
            string fileDataAsString = JsonConvert.SerializeObject(
                fileData, 
                Formatting.Indented
            );
            File.WriteAllText(path, fileDataAsString);
        }
    }
}