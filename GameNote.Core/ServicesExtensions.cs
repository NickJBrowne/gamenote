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
                .AddTransient<IDialogHandler, DialogHandler>();
        }

        public static IServiceCollection AddSingletonSettingsHandler(this IServiceCollection services, ISettingsHandler settingsHandler)
            => services.AddSingleton<ISettingsHandler>(settingsHandler);

        public static IServiceCollection AddAppDataSettingsHandler(this IServiceCollection services)
            => services.AddTransient<ISettingsHandler, AppDataSettingsHandler>();
    }
}