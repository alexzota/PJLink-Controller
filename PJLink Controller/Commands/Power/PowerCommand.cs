namespace PJLink_Controller.Commands.Power
{
    public class PowerCommand : Command
    {
        private PowerInstructionType _requestType { get; set; }
        private PowerStatus _powerStatus { get; set; }

        public PowerCommand(PowerInstructionType requestType)
        {
            _requestType = requestType;
        }

        public override string GetCommand()
        {
            var command = "%1PWR ";

            switch (_requestType)
            {
                case PowerInstructionType.ON:
                    command += "1";
                    break;
                case PowerInstructionType.OFF:
                    command += "2";
                    break;
                case PowerInstructionType.QUERY:
                    command += "?";
                    break;
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
