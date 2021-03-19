using System.Threading.Tasks;
using GameNote.CLI.Helpers;
using GameNote.Core.GameList;
using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = "find", Description = "Find game files within a directory")]
    class FindCommand : BaseCommand
    {
        private readonly GetGamesInDirectory _handler;

        public FindCommand(GetGamesInDirectory handler)
        {
            _handler = handler;
        }

        [Option(CommandOptionType.SingleValue, ShortName = "d", LongName = "directory", Description = "The directory to look in")]
        public string Directory { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            Message($"Looking for games under {Directory}");

            var result = _handler.Run(Directory);
            
            ConsoleWriteTable.New(result)
                .AddColumn("File Name", x => x.Executable)
                .AddColumn("Full Path", x => x.FullPath.Replace(Directory, ""), ColumnAlign.Left)
                .Write();

            return Success();
        }
    }
}