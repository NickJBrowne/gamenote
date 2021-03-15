using FluentAssertions;
using Moq;
using Helpers.UnitTest;
using System;
using GameNote.Core.GameClose;

namespace GameNote.Core.UnitTests.UserStories.GameCloseActionTests
{
    public static class Steps
    {
        private static Uri _testUrl = new Uri("http://www.google.com");
        private static string _testProgram = "notepad.exe";

        public static Given<GameCloseActionContext> That_The_User_Wants_To_Open_URL(this Given<GameCloseActionContext> given)
        {
            given.Context.OnSuccessAction = GameCloseAction.OpenUrl(_testUrl);
            return given;
        }

        public static Given<GameCloseActionContext> That_The_User_Wants_To_Open_Program(this Given<GameCloseActionContext> given)
        {
            given.Context.OnSuccessAction = GameCloseAction.OpenProgram(_testProgram);
            return given;
        }

        public static Given<GameCloseActionContext> That_The_User_Doesnt_Want_Anything_To_Happen(this Given<GameCloseActionContext> given)
        {
            given.Context.OnSuccessAction = GameCloseAction.DoNothing();
            return given;
        }

        public static When<GameCloseActionContext> The_Game_Closes(this When<GameCloseActionContext> when)
        {
            var dialog = new GameCloseDialog(when.Context.DialogHandler.Object);
            when.Context.ActionResult = dialog.Ask(when.Context.OnSuccessAction);
            return when;
        }

        public static Given<GameCloseActionContext> The_Player_Will_Select_Yes_From_Dialog(this Given<GameCloseActionContext> given)
        {
            given.Context.DialogHandler
                .Setup(r => r.AskYesNo(It.IsAny<string>()))
                .Callback(() => given.Context.CalledDialog = true)
                .Returns(DialogResult.Yes);

            return given;
        }

        public static Given<GameCloseActionContext> The_Player_Selects_No_From_Dialog(this Given<GameCloseActionContext> given)
        {
            given.Context.DialogHandler
                .Setup(r => r.AskYesNo(It.IsAny<string>()))
                .Callback(() => given.Context.CalledDialog = true)
                .Returns(DialogResult.No);

            return given;
        }

        public static Then<GameCloseActionContext> Open_The_Url_For_That_Game(this Then<GameCloseActionContext> then)
        { 
            then.Context.ActionResult.Should().NotBeNull();
            then.Context.ActionResult.Arguments.Should().Be(_testUrl.ToString());
            then.Context.ActionResult.Action.Should().Be(GameCloseActionEnum.OpenUrl);
            return then;
        }

        public static Then<GameCloseActionContext> Open_The_Program(this Then<GameCloseActionContext> then)
        {
            then.Context.ActionResult.Should().NotBeNull();
            then.Context.ActionResult.Arguments.Should().Be(_testProgram);
            then.Context.ActionResult.Action.Should().Be(GameCloseActionEnum.OpenProgram);
            return then;
        }

        public static Then<GameCloseActionContext> Do_Nothing(this Then<GameCloseActionContext> then)
        {
            then.Context.ActionResult.Should().NotBeNull();
            then.Context.ActionResult.Action.Should().Be(GameCloseActionEnum.DoNothing);
            return then;
        }

        public static Then<GameCloseActionContext> The_Dialog_Should_Not_Be_Shown(this Then<GameCloseActionContext> then)
        {
            then.Context.CalledDialog.Should().BeFalse();
            return then;
        }
    }
}