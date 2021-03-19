using GameNote.Core.Settings;

namespace GameNote.Core.GameClose
{
    public interface IGameCloseActionHandler
    {
        void Run(GameSetting gameToRun);
    }
}