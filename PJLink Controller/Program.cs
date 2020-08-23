using PJLink_Controller.Commands.InputSource;
using PJLink_Controller.Commands.Power;
using System;
using System.Threading.Tasks;

namespace PJLink_Controller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var projector = new PJLinkProjector("192.0.0.1", "password");

            PowerCommand powerOnCommand = new PowerCommand(PowerInstructionType.ON);
            await projector.SendCommandAsync(powerOnCommand);
            Console.WriteLine($"The projector is {powerOnCommand.CurrentPowerStatus}");



            InputCommand sourceVideoCommand = new InputCommand(InputInstructionType.VIDEO);
            projector.SendCommand(sourceVideoCommand);
            Console.WriteLine($"Source set to: {sourceVideoCommand.CurrentInputSource}");

            InputCommand sourceQueryCommand = new InputCommand(InputInstructionType.QUERY);
            projector.SendCommand(sourceQueryCommand);
            Console.WriteLine($"Current source: {sourceQueryCommand.CurrentInputSource}");




            PowerCommand powerQueryCommand = new PowerCommand(PowerInstructionType.QUERY);
            projector.SendCommand(powerQueryCommand);
            Console.WriteLine($"The projector is {powerQueryCommand.CurrentPowerStatus}");

            PowerCommand powerOffCommand = new PowerCommand(PowerInstructionType.OFF);
            projector.SendCommand(powerOffCommand);
            Console.WriteLine($"The projector is {powerOffCommand.CurrentPowerStatus}");
        }
    }
}
