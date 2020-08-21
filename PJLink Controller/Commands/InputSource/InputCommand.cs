namespace PJLink_Controller.Commands.InputSource
{
    public class InputCommand : Command
    {
        private InputInstructionType _requestType { get; set; }
        private int _port { get; set; } = -1;

        public InputCommand(InputInstructionType requestType, int port = -1)
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
            else if (_port > 0)
            {
                command += _requestType + _port;
            }

            return command;
        }

        public override ResponseType GetResponse(string response)
        {
            var baseResponse = base.GetResponse(response);

            if (baseResponse != ResponseType.SUCCESSFUL_EXECUTION)
            {
                _powerStatus = PowerStatus.UNDEFINED;
                return baseResponse;
            }

            if (_requestType == PowerInstructionType.QUERY)
            {
                response = response.Replace("%1POWR=", "");
                int powerValue = int.Parse(response);

                switch (powerValue)
                {
                    case 0:
                        _powerStatus = PowerStatus.OFF;
                        break;
                    case 1:
                        _powerStatus = PowerStatus.ON;
                        break;
                }
            }

            return ResponseType.SUCCESSFUL_EXECUTION;
        }
    }
}
