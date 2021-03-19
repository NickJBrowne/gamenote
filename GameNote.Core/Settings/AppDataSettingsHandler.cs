using System;
using System.IO;
using Microsoft.Extensions.Options;

namespace GameNote.Core.Settings
{
    public class AppDataSettingsHandler : SettingsHandler, ISettingsHandler
    {
        public AppDataSettingsHandler(IFileSystemHandler fileSystemHandler, IOptions<GameNoteConfiguration> optionsConfiguration)
            : base(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "GameNote"
                ), fileSystemHandler, optionsConfiguration)
        {
            
        }
    }
}