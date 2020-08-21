using PJLink_Controller.Commands.InputSource;
using PJLink_Controller.Commands.Power;
using System;

namespace PJLink_Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            var projector = new PJLinkProjector("192.0.0.1", "password");

            PowerCommand powerOnCommand = new PowerCommand(PowerInstructionType.ON);
            projector.SendCommand(powerOnCommand);
            Console.WriteLine($"The projector is {powerOnCommand._powerStatus}");



            InputCommand sourceVideoCommand = new InputCommand(InputInstructionType.VIDEO);
            projector.SendCommand(sourceVideoCommand);
            Console.WriteLine($"Source set to: {sourceVideoCommand._inputSource}");

            InputCommand sourceQueryCommand = new InputCommand(InputInstructionType.QUERY);
            projector.SendCommand(sourceQueryCommand);
            Console.WriteLine($"Current source: {sourceQueryCommand._inputSource}");




            PowerCommand powerQueryCommand = new PowerCommand(PowerInstructionType.QUERY);
            projector.SendCommand(powerQueryCommand);
            Console.WriteLine($"The projector is {powerQueryCommand._powerStatus}");

            PowerCommand powerOffCommand = new PowerCommand(PowerInstructionType.OFF);
            projector.SendCommand(powerOffCommand);
            Console.WriteLine($"The projector is {powerOffCommand._powerStatus}");
        }
    }
}
