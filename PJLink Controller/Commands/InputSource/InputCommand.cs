using System;

namespace PJLink_Controller.Commands.InputSource
{
    public class InputCommand : Command
    {
        private InputInstructionType _requestType { get; set; }
        public InputSourceType CurrentInputSource { get; set; }

        private int _port { get; set; } = -1;

        public InputCommand(InputInstructionType requestType, int port = -1): base()
        {
            _requestType = requestType;
            _port = port;
        }

        public override string GetCommand()
        {
            var command = "%1INPT ";

            if(_requestType == InputInstructionType.QUERY)
            {
                command += "?";
            }
            else if (_port > 0 && _port < 10)
            {
                command += _requestType + _port;
            }
            else
            {
                throw new Exception();
            }

            return command;
        }

        public override ResponseType GetResponse(string response)
        {
            var baseResponse = base.GetResponse(response);

            if (baseResponse != ResponseType.SUCCESSFUL_EXECUTION)
            {
                CurrentInputSource = InputSourceType.UNDEFINED;
                return baseResponse;
            }

            if (_requestType == InputInstructionType.QUERY)
            {
                response = response.Replace("%1INPT=", "");
                int inputValue = int.Parse(response);

                CurrentInputSource = (InputSourceType)(inputValue / 10);
                _port = inputValue % 10;
            }

            return ResponseType.SUCCESSFUL_EXECUTION;
        }
    }
}
