using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameNote.Core;
using GameNote.Core.GameClose;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameNote.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISettingsHandler _settingsHandler;
        private Dictionary<string, GameCloseAction> _gameCloseActions = new Dictionary<string, GameCloseAction>();

        public Worker(ILogger<Worker> logger, ISettingsHandler settingsHandler)
        {
            _logger = logger;
            this._settingsHandler = settingsHandler;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            LoadOnCloseActions();            
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_gameCloseActions.Any() == false)
                {
                    await StopAsync(stoppingToken);
                    return;
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                if (_settingsHandler.HasChangedSinceLastLoad())
                {
                    _logger.LogDebug("Settings file has been updated");
                    LoadOnCloseActions();
                }
            }
        }

        private void LoadOnCloseActions()
        {
            var settings = _settingsHandler.Load();
            if (settings == null)
                _logger.LogError("No settings data found");

            if (settings.HasGames() == false)
                _logger.LogWarning("No games to find");

            _gameCloseActions = settings.Games
                .ToDictionary(
                    game => Path.GetFileNameWithoutExtension(game.FilePath),
                    game => game.GameCloseAction
                );
        }
    }
}
