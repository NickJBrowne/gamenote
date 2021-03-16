using System;
using System.Threading.Tasks;
using GameNote.CLI.Interfaces;
using GameNote.Core;
using GameNote.Core.GameList;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameNote.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services
                    .AddTransient<GetGamesInDirectory>()
                    .AddTransient<IFileSystemHandler, FileSystemHandler>()
                    .AddTransient<IDialogHandler, DialogHandler>();
                });

            try
            {
                // Game list
                // Add game
                // Remove game
                // Change game
                // Show black list
                return await hostBuilder.RunCommandLineApplicationAsync<GameNoteCmd>(args);
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
