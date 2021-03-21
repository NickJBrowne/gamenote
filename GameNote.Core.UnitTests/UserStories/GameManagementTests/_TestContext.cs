using System.Collections.Generic;
using GameNote.Core.GameList;
using GameNote.Core.Settings;

namespace GameNote.Core.UnitTests.UserStories.GameManagementTests
{
    public class GameManagementContext
    {
        public List<Game> GameList { get; set; }
        public GameNoteSettings Settings { get; set; } = new GameNoteSettings();
    }
}