using System;
using System.Diagnostics;
using GameNote.Core.Settings;

namespace GameNote.Core.GameClose
{
    public class GameCloseActionHandler : IGameCloseActionHandler
    {
        public void Run(GameSetting gameToRun)
        {
            switch (gameToRun.GameCloseAction.Action)
            {
                case GameCloseActionEnum.OpenProgram:
                    OpenProgram(gameToRun);
                    break;

                case GameCloseActionEnum.OpenUrl:
                    OpenUrl(gameToRun);
                    break;

                case GameCloseActionEnum.DoNothing:
                    // Do nothing
                    break; 

                default:
                    throw new NotImplementedException($"No implementation for action: {gameToRun.GameCloseAction.Action}");
            }
        }

        private void OpenUrl(GameSetting gameToRun)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {gameToRun.GameCloseAction.Arguments}") { CreateNoWindow = true });
        }

        private void OpenProgram(GameSetting gameToRun)
        {
            Process.Start(new ProcessStartInfo("cmd", $"{gameToRun.GameCloseAction.Arguments}") { CreateNoWindow = true });
        }
    }
}