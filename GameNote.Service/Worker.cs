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
        private List<string> _games = new List<string>();
        private CancellationToken _stoppingToken;

        public Worker(ILogger<Worker> logger, ISettingsHandler settingsHandler)
        {
            _logger = logger;
            _settingsHandler = settingsHandler;
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
                .Select(game => Path.GetFileName(game.FileName))
                .ToList();

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

            var gameProcess = FindRunningProcess();
            if (gameProcess != null)
                await WaitForFinish(gameProcess, _stoppingToken);

            if (_settingsHandler.HasChangedSinceLastLoad())
            {
                _logger.LogDebug("Settings file has been updated");
                LoadOnCloseActions();
            }
            
            _timer.Start();
        }

        private Process FindRunningProcess()
        {
            foreach(var game in _games)
            {
                var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(game)).ToList();
                var process = processes.FirstOrDefault(p => p.MainModule.FileName.EndsWith(game));
                if (process != null)
                    return process;
            }

            return null;
        }

        private async Task WaitForFinish(Process game, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{game.ProcessName} is running...");
            await game.WaitForExitAsync(cancellationToken);
            _logger.LogInformation($"{game.ProcessName} has exited");

            var settings = _settingsHandler.Load();
            var cli = settings.GetCLI();
            
            if (cli.IsValidPath() == false)
                throw new Exception("Cannot run game because path to CLI is not provided in settings");

            cli.GameRun(game.ProcessName);
        }
    }
}
