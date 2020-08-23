using NSubstitute;
using NSubstitute.Extensions;
using PJLink_Controller;
using PJLink_Controller.Commands;
using PJLink_Controller.Commands.Power;
using Xunit;

namespace PJLinkController.UnitTests
{
    public class PJLinkProjectorTests
    {
        [Fact]
        public void SendCommand_ReturnsError_WhenCommandGetResponseReturnsError()
        {
            //Arrange
            var projector = Substitute.For<PJLinkProjector>();
            Command command = Substitute.For<PowerCommand>(PowerInstructionType.OFF);
            command.GetResponse("").ReturnsForAnyArgs(ResponseType.ERROR);

            //Act
            var response = projector.SendCommand(command);

            //Assert
            Assert.Equal(ResponseType.ERROR, response);
        }
    }
}
