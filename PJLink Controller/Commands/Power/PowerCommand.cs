namespace PJLink_Controller.Commands.Power
{
    public class PowerCommand : Command
    {
        public PowerInstructionType _requestType { get; set; }
        public PowerStatus CurrentPowerStatus { get; set; }

        public PowerCommand(PowerInstructionType requestType): base()
        {
            _requestType = requestType;
        }

        public override string GetCommand()
        {
            var command = "%1PWR ";

            switch (_requestType)
            {
                case PowerInstructionType.OFF:
                    command += "0";
                    break;
                case PowerInstructionType.ON:
                    command += "1";
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
                CurrentPowerStatus = PowerStatus.UNDEFINED;
                return baseResponse;
            }

            if (_requestType == PowerInstructionType.QUERY)
            {
                response = response.Replace("%1POWR=", "");
                int powerValue = int.Parse(response);

                switch (powerValue)
                {
                    case 0:
                        CurrentPowerStatus = PowerStatus.OFF;
                        break;
                    case 1:
                        CurrentPowerStatus = PowerStatus.ON;
                        break;
                    default:
                        CurrentPowerStatus = PowerStatus.UNDEFINED;
                        break;
                }
            }

            return ResponseType.SUCCESSFUL_EXECUTION;
        }
    }
}
