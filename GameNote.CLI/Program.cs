using System;
using System.IO;
using System.Linq;
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
            var settingsHandler = new SettingsHandler(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "GameNote"
                )
            );
            var settings = settingsHandler.Load();

            var services = new ServiceCollection()
                .AddTransient<GetGamesInDirectory>()
                .AddTransient<GameSettingBuilder>()
                .AddTransient<IFileSystemHandler, FileSystemHandler>()
                .AddTransient<IDialogHandler, DialogHandler>()
                .AddSingleton<ISettingsHandler>(settingsHandler)
                .BuildServiceProvider();

            try
            {
                var app = new CommandLineApplication()
                {
                    FullName = "GameNote Command Line Interface",
                    ShortVersionGetter = () => $"v{typeof(Settings).Assembly.GetName().Version}"
                };

                Console.WriteLine(app.GetFullNameAndVersion());
                Console.WriteLine();

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
                       listCommand.Description = "Show a list of all games being monitored";

                       listCommand.OnExecute(() => {
                            var settings = settingsHandler.Load();

                            if (settings.Games.Any() == false)
                            {
                                Console.WriteLine("No games found"); 
                                return 0;
                            }

                            new ConsoleWriteTable<Settings.GameSetting>(settings.Games)
                                .AddColumn("File Name", g => Path.GetFileName(g.FilePath))
                                .AddColumn("File Path", g => g.FilePath, ConsoleWriteTable<Settings.GameSetting>.ColumnAlign.Left)
                                .AddColumn("OnCloseAction", g => g.GameCloseAction.Action.ToString())
                                .AddColumn("Arguments", g => g.GameCloseAction.Arguments)
                                .Write();
                            return 0;
                       });
                    });

                    gameCommand.Command("find", findCommand => {
                        findCommand.HelpOption();
                        findCommand.Description = "Find potential executable files in a directory";

                        var directory = findCommand
                            .Option("-d|--dir", "Directory to look in", CommandOptionType.SingleValue)
                            .IsRequired();

                        findCommand.OnExecute(() => {
                            var handler = services.GetRequiredService<GetGamesInDirectory>();
                            var result = handler.Run(directory.Value());
                            new ConsoleWriteTable<Game>(result)
                                .AddColumn("File Name", x => x.Executable)
                                .AddColumn("Full Path", x => x.FullPath, ConsoleWriteTable<Game>.ColumnAlign.Left)
                                .Write();
                            return 0;
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

                        var openUrl = addCommand
                            .Option("-url|--open-url", "On game close open a browser to url", CommandOptionType.SingleValue);

                        var openProgram = addCommand
                            .Option("-exe|--open-program", "On game close open a program", CommandOptionType.SingleValue);

                        addCommand.OnExecute(() => {
                            var builder = services.GetRequiredService<GameSettingBuilder>();

                            if (directPath.HasValue())
                                builder.FromFullPath(directPath.Value());
                            else if (folder.HasValue() && exeName.HasValue())
                                builder.WithinDirectory(folder.Value(), exeName.Value());
                            else
                                throw new Exception("You must specify either a direct file path or a folder path and a file name");

                            if (openUrl.HasValue())
                                builder.OnGameCloseOpenUrl(openUrl.Value());
                            else if (openProgram.HasValue())
                                builder.OnGameCloseOpenProgram(openProgram.Value());
                            else
                            {
                                Console.WriteLine("On game close, nothing will happen");
                                builder.OnGameCloseDoNothing();
                            }

                            string fileName = builder.Build();
                            Console.WriteLine($"Added game: {fileName}");
                            return 0;
                        });
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
