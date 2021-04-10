using System;
using System.Diagnostics;
using GameNote.Core.Settings;

namespace GameNote.Core.GameClose
{
    public class GameCloseActionHandler : IGameCloseActionHandler
    {
        public void Run(GameSetting gameToRun, Action<string> loggerHandler)
        {
            switch (gameToRun.GameCloseAction.Action)
            {
                case GameCloseActionEnum.OpenProgram:
                    OpenProgram(gameToRun, loggerHandler);
                    break;

                case GameCloseActionEnum.OpenUrl:
                    OpenUrl(gameToRun, loggerHandler);
                    break;

                case GameCloseActionEnum.DoNothing:
                    loggerHandler.Invoke("Doing nothing");
                    // Do nothing
                    break; 

                default:
                    throw new NotImplementedException($"No implementation for action: {gameToRun.GameCloseAction.Action}");
            }
        }

        private void OpenUrl(GameSetting gameToRun, Action<string> loggerHandler)
        {
            loggerHandler.Invoke($"Opening URL: {gameToRun.GameCloseAction.Arguments}");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {gameToRun.GameCloseAction.Arguments}") { CreateNoWindow = true });
        }

        private void OpenProgram(GameSetting gameToRun, Action<string> loggerHandler)
        {
            loggerHandler.Invoke($"Running command: {gameToRun.GameCloseAction.Arguments}");
            Process.Start(new ProcessStartInfo("cmd", $"{gameToRun.GameCloseAction.Arguments}") { CreateNoWindow = true });
        }
    }
}