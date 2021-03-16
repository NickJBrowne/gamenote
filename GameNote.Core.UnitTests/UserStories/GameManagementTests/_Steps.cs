using System.Collections.Generic;
using System.Linq;
using Helpers.UnitTest;
using System.IO;
using FluentAssertions;
using GameNote.Core.GameList;
using Moq;
using GameNote.Core.GameClose;

namespace GameNote.Core.UnitTests.UserStories.GameManagementTests
{
    public static class Steps
    {
        private static string _setupGame = @"C:\folder4\setupgame.exe";

        private static List<string> _files = new List<string>() {
            @"C:\folder\game.exe",
            _setupGame,
            @"C:\folder2\CrashLoader.exe",
            @"C:\folder3\vc_redist.exe"
        };       

        private static string _validDirectory = Directory.GetCurrentDirectory();

        public static Given<GameManagementContext> The_User_Wants_To_Look_In_A_Directory_For_Games(
            this Given<GameManagementContext> given
        )
        {
            return given;
        }

        public static Given<GameManagementContext> The_User_Has_Already_Setup_A_Game(
            this Given<GameManagementContext> given
        )
        {
            given.Context.Settings.AddGame(_setupGame, GameCloseAction.OpenUrl(new System.Uri("http://test.com")));
            return given;
        }

        public static When<GameManagementContext> The_List_Is_Returned(
            this When<GameManagementContext> when
        )
        {
            var mock = new Mock<IFileSystemHandler>();
            mock.Setup(r => r.DoesDirectoryExist(It.IsAny<string>()))
                .Returns(true);

            mock.Setup(r => r.GetExecutableFiles(It.IsAny<string>()))
                .Returns(_files.Select(f => new FileInfo(f)));

            when.Context.GameList = new GetGamesInDirectory(
                mock.Object,
                when.Context.Settings
            ).Run(_validDirectory).ToList();
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
            string fileName = Path.GetFileName(_setupGame);
            var setupGame = then.Context.GameList.FirstOrDefault(g => g.Executable == fileName);
            setupGame.Should().NotBeNull();
            setupGame.AlreadyConfigured.Should().BeTrue();
            return then;
        }
    }
}