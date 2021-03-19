using McMaster.Extensions.CommandLineUtils;

namespace GameNote.CLI.Commands.Game
{
    [Command(Name = "game", Description = "Game options")]
    [Subcommand(
        typeof(ListCommand),
        typeof(FindCommand),
        typeof(AddGameCommand),
        typeof(RunCommand)
    )]
    class BaseGameCommand : BaseCommand
    {
        
    }
}