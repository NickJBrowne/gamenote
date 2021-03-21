using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GameNote.CLI.Commands;
using GameNote.CLI.Helpers;
using GameNote.Core;
using GameNote.Core.GameClose;
using GameNote.Core.GameList;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameNote.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {            
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddGameNoteServices()
                        .AddAppDataSettingsHandler()
                        .AddTransient<IDialogHandler, DialogHandler>()
                        .BuildServiceProvider();
                });

            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                Console.WriteLine($"GameNote CLI ({version.Major}.{version.Minor})\r\n");

                return await builder
                    .RunCommandLineApplicationAsync<GameNoteCommand>(args);
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
