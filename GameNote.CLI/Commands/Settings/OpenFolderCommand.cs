using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Settings
{
    [Command(Name = CommandKeys.Settings_OpenFolder, Description = "Open the folder that has the settings file")]
    class OpenFolderCommand : BaseCommand
    {
        private readonly ISettingsHandler _settingsHandler;

        public OpenFolderCommand(ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            string pathToSettingsFile = _settingsHandler.GetPathToSettingsFile();

            Message($"Settings file found: {pathToSettingsFile}");
            Process.Start("explorer.exe", Path.GetDirectoryName(pathToSettingsFile));
            return Success();
        }
    }
}