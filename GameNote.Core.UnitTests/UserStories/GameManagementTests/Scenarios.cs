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
            .When()
                .The_List_Is_Returned()
            .Then()
                .It_Should_Not_Show_Any_Items_On_The_File_Name_Blacklist()
                .And.It_Should_Show_Which_Ones_Have_Already_Been_Setup();
        }

        [Fact]
        public void AddItem()
        {
            // Given
                // There is a file path
                // And the user selects a game action for them
            // When
                // The user adds the game
            // Then 
                // it should add it to the game list
        }

        [Fact]
        public void ChangeItem()
        {
            // Given
                // The user has already saved settings for game
                // And the user wants to change the game action for them
            // When
                // The user saves the game
            // Then
                // It should update it to the game list
        }

        [Fact]
        public void RemoveItem()
        {
            // Given
                // The user has already saved settings for the game
            // When
                // The user chooses to delete it
            // Then
                // It should be removed from the game settings
        }
    }
}