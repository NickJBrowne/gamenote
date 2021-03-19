using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GameNote.CLI.Helpers;
using GameNote.Core;
using GameNote.Core.GameClose;
using GameNote.Core.GameList;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace GameNote.CLI
{
    class Program
    {
        static int Main(string[] args)
        {            
            var services = new ServiceCollection()
                .AddGameNoteServices()
                .AddAppDataSettingsHandler()
                .AddPathToCLI(Environment.CurrentDirectory)
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
                            var settingsHandler = services.GetRequiredService<ISettingsHandler>();
                            var settings = settingsHandler.Load();

                            if (settings.Games.Any() == false)
                            {
                                Console.WriteLine("No games found"); 
                                return 0;
                            }

                            new ConsoleWriteTable<GameSetting>(settings.Games)
                                .AddColumn("File Name", g => Path.GetFileName(g.FilePath))
                                .AddColumn("File Path", g => g.FilePath, ConsoleWriteTable<GameSetting>.ColumnAlign.Left)
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
                            .Option("-d|--directory", "Directory to look in", CommandOptionType.SingleValue)
                            .IsRequired();

                        findCommand.OnExecute(() => {
                            Console.WriteLine($"Looking for games under {directory.Value()}");
                            var handler = services.GetRequiredService<GetGamesInDirectory>();
                            var result = handler.Run(directory.Value());
                            new ConsoleWriteTable<Game>(result)
                                .AddColumn("File Name", x => x.Executable)
                                .AddColumn("Full Path", x => x.FullPath.Replace(directory.Value(), ""), ConsoleWriteTable<Game>.ColumnAlign.Left)
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
                            .Option("-d|--directory", "Folder to look under", CommandOptionType.SingleValue);
                        
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

                    gameCommand.Command("run", runCommand => {
                        runCommand.HelpOption();
                        runCommand.Description = "Run the game close action for a game";

                        var game = runCommand
                            .Option("-g|--game", "The name of the game executable", CommandOptionType.SingleValue)
                            .IsRequired();

                        runCommand.OnExecute(() => {
                            var settingsHandler = services.GetRequiredService<ISettingsHandler>(); 
                            var settings = settingsHandler.Load();

                            var gameToRun = settings.FindGame(game.Value());
                            if (gameToRun == null)
                            {
                                Console.WriteLine($"No game found for: {game.Value()}");   
                                return 1;
                            }

                            if (gameToRun.GameCloseAction.Action == GameCloseActionEnum.DoNothing)
                            {
                                Console.WriteLine("Doing nothing...");
                                return 1;
                            }

                            Console.WriteLine($"Found Game setting for {gameToRun.FileName}");

                            var closeActionHandler = services.GetRequiredService<IGameCloseActionHandler>();
                            closeActionHandler.Run(gameToRun);
                            return 0;
                        });
                    });
                });

                app.Command("settings", settingsCommand => {
                    settingsCommand.HelpOption();
                    settingsCommand.Description = "Actions for the settings setup for GameNote";

                    settingsCommand.OnExecute(() =>
                    {
                        Console.WriteLine("Specify a subcommand");
                        settingsCommand.ShowHelp();
                        return 1;
                    });

                    settingsCommand.Command("open-folder", openFolderCommand => {
                        openFolderCommand.HelpOption();
                        openFolderCommand.Description = "Open the folder where the settings file is stored";

                        openFolderCommand.OnExecute(() => {
                            var settingsHandler = services.GetRequiredService<ISettingsHandler>();
                            string pathToSettingsFile = settingsHandler.GetPathToSettingsFile();

                            Console.WriteLine($"Settings file found: {pathToSettingsFile}");
                            Process.Start("explorer.exe", Path.GetDirectoryName(pathToSettingsFile));
                        });
                    });
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
