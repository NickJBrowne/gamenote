using Xunit;
using Helpers.UnitTest;

namespace GameNote.Core.UnitTests.UserStories.GameCloseActionTests
{
    public class Scenarios : BaseGiven<GameCloseActionContext>
    {
        [Fact]
        public void OpenUrl()
        {
            Given()
                .That_The_User_Wants_To_Open_URL()
                .And.The_Player_Will_Select_Yes_From_Dialog()
            .When()
                .The_Game_Closes()                
            .Then()
                .Open_The_Url_For_That_Game();
        }  

        [Fact]
        public void OpenProgram()
        {
            Given()
                .That_The_User_Wants_To_Open_Program()
                .And.The_Player_Will_Select_Yes_From_Dialog()
            .When()
                .The_Game_Closes()
            .Then()
                .Open_The_Program();
        }

        [Fact]
        public void DoNothing()
        {
            Given()
                .That_The_User_Doesnt_Want_Anything_To_Happen()
            .When()
                .The_Game_Closes()
            .Then()
                .Do_Nothing()
                .And.The_Dialog_Should_Not_Be_Shown();
        }

        [Fact]
        public void SelectNo()
        {
            Given()
                .That_The_User_Wants_To_Open_URL()
                .And.The_Player_Selects_No_From_Dialog()
            .When()
                .The_Game_Closes()                
            .Then()
                .Do_Nothing();
        }
    }
}