using System;
using CommandLine;
using GameNote.CLI.Interfaces;
using GameNote.Core;
using GameNote.Core.GameList;
using Microsoft.Extensions.DependencyInjection;

namespace GameNote.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new ServiceCollection()
                .AddTransient<GetGamesInDirectory>()
                .AddTransient<IFileSystemHandler, FileSystemHandler>()
                .AddTransient<IDialogHandler, DialogHandler>();

            var services = collection.BuildServiceProvider();

            Parser.Default
                .ParseArguments<Verbs.ListGamesVerb>(args)
                .WithParsed<Verbs.ListGamesVerb>(listGames => {
                    var handler = services.GetRequiredService<GetGamesInDirectory>();
                    var result = handler.Run(listGames.Directory, listGames.ShowBlacklist);
                });
                // Add game
                // Remove game
                // Change game
                // Show black list
        }
    }
}
