using System.Collections.Generic;
using System.Linq;
using Helpers.UnitTest;
using System.IO;
using FluentAssertions;
using GameNote.Core.GameList;

namespace GameNote.Core.UnitTests.UserStories.GameManagementTests
{
    public static class Steps
    {
        private static List<string> files = new List<string>() {
            "C:/folder/game.exe",
            "C:/folder4/setupgame.exe",
            "C:/folder2/CrashLoader.exe",
            "C:/folder3/vc_resdist.exe"
        };

        private static string _validDirectory = Directory.GetCurrentDirectory();

        public static Given<GameManagementContext> The_User_Wants_To_Look_In_A_Directory_For_Games(
            this Given<GameManagementContext> given
        )
        {
            return given;
        }

        public static When<GameManagementContext> The_List_Is_Returned(
            this When<GameManagementContext> when
        )
        {
            

            when.Context.GameList = new GetGamesInDirectory(

            ).Run(_validDirectory);
            return when;
        }

        public static Then<GameManagementContext> It_Should_Not_Show_Any_Items_On_The_File_Name_Blacklist(
            this Then<GameManagementContext> then
        )
        {
            then.Context.GameList.Count().Should().Be(2);
            return then;
        }

        public static Then<GameManagementContext> It_Should_Show_Which_Ones_Have_Already_Been_Setup(
            this Then<GameManagementContext> then
        )
        {
            var setupGame = then.Context.GameList.FirstOrDefault(g => g.Executable == "setupgame.exe");
            setupGame.Should().NotBeNull();
            setupGame.AlreadyConfigured.Should().BeTrue();
            return then;
        }
    }
}