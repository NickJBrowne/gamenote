using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = "add", Description = "Add a game")]
    class AddGameCommand : BaseCommand
    {
        private readonly GameSettingBuilder _builder;

        public AddGameCommand(GameSettingBuilder builder)
        {
            _builder = builder;
        }

        [Option(CommandOptionType.SingleValue, ShortName = "p", LongName = "full-path", Description = "The full path to the game")]
        public string DirectPath { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "d", LongName = "directory", Description = "A directory to look under, use with -exe|--file-name")]
        public string Directory { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "e", LongName = "exe", Description = "The name of the file to look for under the directory, use with -d|--directory")]
        public string FileName { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "url", Description = "The url to open when the game closes")]
        public string OpenUrl { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "cmd", Description = "The command to run through command prompt")]
        public string Command { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            if (DirectPath.HasValue())
                _builder.FromFullPath(DirectPath);
            else if (Directory.HasValue() && FileName.HasValue())
                _builder.WithinDirectory(Directory, FileName);
            else
                return Fail("You must specify either a direct file path or a folder path and a file name");

            if (OpenUrl.HasValue())
                _builder.OnGameCloseOpenUrl(OpenUrl);
            else if (Command.HasValue())
                _builder.OnGameCloseOpenProgram(Command);
            else
            {
                Message("On game close, nothing will happen");
                _builder.OnGameCloseDoNothing();
            }

            string fileName = _builder.Build();
            SuccessMessage($"Added game: {fileName}");
            return Success();
        }
    }
}