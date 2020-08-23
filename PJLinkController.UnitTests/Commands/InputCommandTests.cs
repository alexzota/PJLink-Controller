using PJLink_Controller.Commands.InputSource;
using System;
using Xunit;

namespace PJLinkController.UnitTests.Commands
{
    public class InputCommandTests
    {
        [Fact]
        public void GetCommand_VideoInstruction_ShouldReturnValidString()
        {
            //Arrange
            var command = new InputCommand(InputInstructionType.VIDEO, 1);

            //Act
            var cmdString = command.GetCommand();

            //Assert
            Assert.Equal("%1INPT 21", cmdString);
        }

        [Fact]
        public void GetCommand_InvalidPort_ThrowsException()
        {
            //Arrange
            var command = new InputCommand(InputInstructionType.VIDEO, 11);

            //Act && Assert
            Assert.Throws<Exception>(() => command.GetCommand());
        }
    }
}
