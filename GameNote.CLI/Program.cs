using System;
using System.IO;
using CommandLine;
using GameNote.CLI.Helpers;
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

            var parser = new Parser(with => with.EnableDashDash = true);
            parser
                .ParseArguments<
                    Verbs.ListGamesVerb,
                    Verbs.AddGameVerb
                >(args)
                .WithParsed<Verbs.ListGamesVerb>(listGames => {
                    var handler = services.GetRequiredService<GetGamesInDirectory>();
                    var result = handler.Run(listGames.Directory, listGames.ShowBlacklist);
                    new ConsoleWriteTable<Game>(result)
                        .AddColumn("FileName", x => Path.GetFileName(x.FullPath))
                        .AddColumn("Path", x => x.FullPath)
                        .AddColumn("Is Configured", x => x.AlreadyConfigured ? "Yes" : "No")
                        .Write();
                })
                .WithParsed<Verbs.AddGameVerb>(addGame => {});

                // Add game
                // Remove game
                // Change game
                // Show black list
        }
    }
}
