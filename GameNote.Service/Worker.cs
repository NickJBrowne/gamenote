using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        private System.Timers.Timer _timer;
        private Dictionary<string, GameCloseAction> _games = new Dictionary<string, GameCloseAction>();
        private int _gameLastIndex = 0;
        private CancellationToken _stoppingToken;

        public Worker(ILogger<Worker> logger, ISettingsHandler settingsHandler)
        {
            _logger = logger;
            this._settingsHandler = settingsHandler;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            LoadOnCloseActions();            
            _timer = new System.Timers.Timer(2000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _stoppingToken = cancellationToken;
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            _timer.Dispose();
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Executing");
                _timer.Start();
            }
            catch(Exception ex)
            {
                _timer.Stop();
                _logger.LogCritical(ex, "Error during Execution");
            }
        }

        private void LoadOnCloseActions()
        {
            var settings = _settingsHandler.Load();
            if (settings == null)
                _logger.LogError("No settings data found");

            if (settings.HasGames() == false)
                _logger.LogWarning("No games to find");

            _games = settings.Games
                .Where(game => game.GameCloseAction.Action != GameCloseActionEnum.DoNothing)
                .ToDictionary(
                    game => Path.GetFileName(game.FilePath),
                    game => game.GameCloseAction
                );
            _gameLastIndex = _games.Count();
            _logger.LogInformation($"{_games.Count} games found");
        }
        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (_games.Any() == false)
            {
                _logger.LogWarning("No games detecting");
                return;
            }

            var (gameProcess, onCloseAction) = FindRunningProcess();
            if (gameProcess != null && onCloseAction != null)
                await WaitForFinish(gameProcess, onCloseAction, _stoppingToken);

            if (_settingsHandler.HasChangedSinceLastLoad())
            {
                _logger.LogDebug("Settings file has been updated");
                LoadOnCloseActions();
            }
            
            _timer.Start();
        }

        private (Process, GameCloseAction) FindRunningProcess()
        {
            foreach(var game in _games)
            {
                var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(game.Key)).ToList();
                var process = processes.FirstOrDefault(p => p.MainModule.FileName.EndsWith(game.Key));
                if (process != null)
                    return (process, game.Value);
            }

            return (null, null);
        }

        private async Task WaitForFinish(Process game, GameCloseAction gameCloseAction, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{game.ProcessName} is running...");
            await game.WaitForExitAsync(cancellationToken);
            _logger.LogInformation($"{game.ProcessName} has exited");

            if (gameCloseAction.Action == GameCloseActionEnum.OpenUrl)
                Process.Start(new ProcessStartInfo("cmd", $"/c start {gameCloseAction.Arguments}") { CreateNoWindow = true });
            else if (gameCloseAction.Action == GameCloseActionEnum.OpenProgram)
                Process.Start(new ProcessStartInfo("cmd", $"{gameCloseAction.Arguments}") { CreateNoWindow = true });
        }
    }
}
