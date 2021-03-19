using GameNote.Core.GameClose;
using GameNote.Core.GameList;
using Microsoft.Extensions.DependencyInjection;

namespace GameNote.Core
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddGameNoteServices(this IServiceCollection services)
        {
            return services
                .AddTransient<GetGamesInDirectory>()
                .AddTransient<GameSettingBuilder>()
                .AddTransient<IFileSystemHandler, FileSystemHandler>()
                .AddTransient<IDialogHandler, DialogHandler>()
                .AddTransient<IGameCloseActionHandler, GameCloseActionHandler>();
        }

        public static IServiceCollection AddPathToCLI(this IServiceCollection services, string path)
        {
            services
                .AddOptions<Configuration>()
                .Configure(configureOptions => configureOptions.PathToCLI = path);

            return services;
        }

        public static IServiceCollection AddAppDataSettingsHandler(this IServiceCollection services)
            => services.AddTransient<ISettingsHandler, AppDataSettingsHandler>();
    }
}