using System.Collections.Generic;
using GameNote.Core.GameList;

namespace GameNote.Core.UnitTests.UserStories.GameManagementTests
{
    public class GameManagementContext
    {
        public List<Game> GameList { get; set; }
        public Settings Settings { get; set; } = new Settings("/Path/To/CLI");
    }
}