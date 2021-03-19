using Helpers.UnitTest;
using Xunit;

namespace GameNote.Core.UnitTests.UserStories.GameManagementTests
{
    public class Scenarios : BaseGiven<GameManagementContext>
    {
        [Fact]
        public void GetList()
        {
            Given()
                .The_User_Wants_To_Look_In_A_Directory_For_Games()
                .And.The_User_Has_Already_Setup_A_Game()
            .When()
                .The_List_Is_Returned()
            .Then()
                .It_Should_Not_Show_Any_Items_On_The_File_Name_Blacklist()
                .And.It_Should_Show_Which_Ones_Have_Already_Been_Setup();
        }
    }
}