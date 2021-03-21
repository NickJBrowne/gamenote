using GameNote.Core.GameClose;
using GameNote.Core.GameList;
using GameNote.Core.Settings;
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
                .AddTransient<GameCloseDialog>()
                .AddTransient<IFileSystemHandler, FileSystemHandler>()
                .AddTransient<IGameCloseActionHandler, GameCloseActionHandler>();
        }

        public static IServiceCollection AddAppDataSettingsHandler(this IServiceCollection services)
            => services.AddTransient<ISettingsHandler, AppDataSettingsHandler>();
    }
}