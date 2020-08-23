namespace PJLink_Controller.Commands
{
    public class Command
    {
        public ResponseType ResponseType { get; set; }

        public Command()
        {
        }

        public virtual ResponseType GetResponse(string response)
        {
            System.Console.WriteLine($"DEBUG - Started processing response: {response}");

            if (response.Contains("=ERR1"))
            {
                ResponseType = ResponseType.UNDEFINED_COMMAND;
            }
            else if (response.Contains("=ERR2"))
            {
                ResponseType = ResponseType.OUT_OF_PARAMETER;
            }
            else if (response.Contains("=ERR3"))
            {
                ResponseType = ResponseType.UNVAILABLE_TIME;
            }
            else if (response.Contains("=ERR4"))
            {
                ResponseType = ResponseType.PROJECTOR_FAILURE;
            }
            else if (response.Contains(" ERRA"))
            {
                ResponseType = ResponseType.AUTH_FAILURE;
            }
            else
            {
                ResponseType = ResponseType.SUCCESSFUL_EXECUTION;
            }

            System.Console.WriteLine($"DEBUG - Finished processing response: {response}");

            return ResponseType;
        }

        public virtual string GetCommand()
        {
            return "";
        }
    }
}
