using Helpers.UnitTest;

namespace GameNote.Core.UnitTests.UserStories.GameManagementTests
{
    public static class Steps
    {
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
            return when;
        }

        public static Then<GameManagementContext> It_Should_Not_Show_Any_Items_On_The_File_Name_Blacklist(
            this Then<GameManagementContext> then
        )
        {
            return then;
        }

        public static Then<GameManagementContext> It_Should_Show_Which_Ones_Have_Already_Been_Setup(
            this Then<GameManagementContext> then
        )
        {
            return then;
        }
    }
}