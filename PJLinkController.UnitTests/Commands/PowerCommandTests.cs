using PJLink_Controller.Commands.Power;
using Xunit;

namespace PJLinkController.UnitTests.Commands
{
    public class PowerCommandTests
    {
        [Fact]
        public void GetCommand_VideoInstruction_ShouldReturnValidString()
        {
            //Arrange
            var command = new PowerCommand(PowerInstructionType.ON);

            //Act
            var cmdString = command.GetCommand();

            //Assert
            Assert.Equal("%1PWR 1", cmdString);
        }
    }
}