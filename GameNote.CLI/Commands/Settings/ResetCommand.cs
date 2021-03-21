using System;
using System.IO;
using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace GameNote.CLI.Commands.Settings
{
    [Command(Name = "reset", Description = "Regenerate the settings command")]
    class ResetCommand : BaseCommand
    {
        private readonly GameNoteConfiguration _config;
        private readonly ISettingsHandler _settingsHandler;

        public ResetCommand(IOptions<GameNoteConfiguration> config, ISettingsHandler settingsHandler)
        {
            _config = config.Value;
            _settingsHandler = settingsHandler;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            string path = _settingsHandler.GetPathToSettingsFile();

            if (File.Exists(path) == false)
                ErrorMessage($"Settings file does not exist: {path}");
            else
            {
                File.Delete(path);
                Message($"File at {path} is deleted");
            }

            var settings = new GameNoteSettings();
            _settingsHandler.Save(settings);
            Message($"Created new settings file");

            return Success();
        }
    }
}