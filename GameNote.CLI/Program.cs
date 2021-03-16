using System;
using System.IO;
using System.Threading.Tasks;
using GameNote.CLI.Helpers;
using GameNote.CLI.Interfaces;
using GameNote.Core;
using GameNote.Core.GameList;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameNote.CLI
{
    class Program
    {
        static int Main(string[] args)
        {            
            var settings = new Settings();

            var services = new ServiceCollection()
                .AddTransient<GetGamesInDirectory>()
                .AddTransient<IFileSystemHandler, FileSystemHandler>()
                .AddTransient<IDialogHandler, DialogHandler>()
                .AddSingleton(settings)
                .BuildServiceProvider();

            try
            {
                var app = new CommandLineApplication()
                {
                    FullName = "GameNote Command Line Interface",
                };

                app.HelpOption();
                app.Command("game", gameCommand => {
                    gameCommand.HelpOption();
                    gameCommand.OnExecute(() =>
                    {
                        Console.WriteLine("Specify a subcommand");
                        gameCommand.ShowHelp();
                        return 1;
                    });

                    gameCommand.Command("list", listCommand => {
                        listCommand.HelpOption();
                        listCommand.Description = "List potential executable files in a directory";

                        var directory = listCommand
                            .Option("-d|--dir", "Directory to look in", CommandOptionType.SingleValue)
                            .IsRequired();

                        listCommand.OnExecute(() => {
                            var handler = services.GetRequiredService<GetGamesInDirectory>();
                            var result = handler.Run(directory.Value());
                            new ConsoleWriteTable<Game>(result)
                                .AddColumn("File Name", x => x.Executable)
                                .AddColumn("Full Path", x => x.FullPath, ConsoleWriteTable<Game>.ColumnAlign.Left)
                                .Write();
                        });
                    });
                    
                    gameCommand.Command("add", addCommand => {
                        addCommand.HelpOption();
                        addCommand.Description = "Add a game to be watched by the service";

                        var directPath = addCommand
                            .Option("-p|--path", "Path to executable", CommandOptionType.SingleValue);

                        var folder = addCommand
                            .Option("-f|--f", "Folder to look under", CommandOptionType.SingleValue);
                        
                        var exeName = addCommand
                            .Option("-fn|--filename", "The name of the file, use this in junction with -f|--f command", CommandOptionType.SingleValue);

                        var onCloseAction = addCommand
                            .Option("-oc|--on-close", "Either open a url or run an application. Use the -a|--args args value to specify details", CommandOptionType.SingleValue)
                            .IsRequired();

                        var args = addCommand
                            .Option("-a|--args", "On close arguments", CommandOptionType.SingleValue)
                            .IsRequired();
                    });
                    // Remove game
                    // Change game
                });

                app.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    app.ShowHelp();
                    return 1;
                });
                
                return app.Execute(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine("\t" + ex.StackTrace);
                Console.ResetColor();
                return 1;
            }
        }
    }
}
