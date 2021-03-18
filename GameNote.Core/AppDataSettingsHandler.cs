using System;
using System.IO;

namespace GameNote.Core
{
    public class AppDataSettingsHandler : SettingsHandler, ISettingsHandler
    {
        public AppDataSettingsHandler(IFileSystemHandler fileSystemHandler)
            : base(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "GameNote"
                ), fileSystemHandler)
        {
            
        }
    }
}