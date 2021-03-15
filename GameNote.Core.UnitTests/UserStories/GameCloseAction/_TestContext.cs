using GameNote.Core.GameClose;
using Moq;

namespace GameNote.Core.UnitTests.UserStories.GameCloseActionTests
{
    public class GameCloseActionContext
    {
        public GameCloseAction OnSuccessAction { get; set; }
        public GameCloseAction ActionResult { get; set; }
        public bool CalledDialog { get; set; } = false;
        public Mock<IDialogHandler> DialogHandler { get; set; } = new Mock<IDialogHandler>();
    }
}