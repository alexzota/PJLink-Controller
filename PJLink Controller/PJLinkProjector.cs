using PJLink_Controller.Commands;
using PJLink_Controller.Commands.InputSource;
using PJLink_Controller.Commands.Power;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PJLink_Controller
{
    public class PJLinkProjector : IDevice
    {
        private TcpClient _client { get; set; }
        private NetworkStream _stream { get; set; }
        private string _hostAddress { get; set; } = "";
        private int _hostPort { get; set; } = 4352;
        private string _password { get; set; } = "";
        private string _sequence { get; set; } = "";

        public PJLinkProjector(string hostAddress, int hostPort, string password)
        {
            _hostAddress = hostAddress;
            _hostPort = hostPort;
            _password = password;
        }

        public PJLinkProjector(string hostAddress, string password)
        {
            _hostAddress = hostAddress;
            _password = password;
        }

        public PJLinkProjector(string password)
        {
            _password = password;
        }
        public PJLinkProjector()
        {
        }

        public ResponseType SendCommand(Command command)
        {
            Console.WriteLine($"DEBUG - Started process to send command {command}");
            try
            {
                if (_client == null || _client.Connected == false)
                {
                    InitializeConnection();
                }
                if (_client == null || _client.Connected == false)
                {
                    return ResponseType.ERROR;
                }

                var commandString = command.GetCommand();
                commandString = CreateMD5(_sequence + _password) + commandString;

                byte[] requestBytes = Encoding.ASCII.GetBytes(commandString);
                _stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] responseBytes = new byte[_client.ReceiveBufferSize];
                int noOfBytesReceived = _stream.Read(responseBytes, 0, _client.ReceiveBufferSize);

                string returndata = Encoding.ASCII.GetString(responseBytes, 0, noOfBytesReceived).Trim();
                Console.WriteLine($"DEBUG - Finished sending command {command}");

                return command.GetResponse(returndata);
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<ResponseType> SendCommandAsync(Command command)
        {
            var response = await Task.Run(() => SendCommand(command));

            return response;
        }

        /// <summary>
        /// As a tcp client connection cannot be reused after closing, we will instantiate another instance everytime
        /// </summary>
        /// <returns></returns>
        private bool InitializeConnection()
        {
            Console.WriteLine($"DEBUG - Trying to open projector connection");

            try
            {
                if (_client == null || _client.Connected == false)
                {
                    _client = new TcpClient(_hostAddress, _hostPort);
                    _stream = _client.GetStream();

                    byte[] receivedBytes = new byte[_client.ReceiveBufferSize];
                    var noOfBytesReceived = _stream.Read(receivedBytes, 0, _client.ReceiveBufferSize);

                    var response = Encoding.ASCII.GetString(receivedBytes, 0, noOfBytesReceived).Trim();

                    //In the mail it's said that the projector is always protected by a password so the response will be PJLINK 1 x-x always
                    _sequence = response.Substring(8);

                    Console.WriteLine($"DEBUG - Opened projector connection");

                    return true;
                }

                Console.WriteLine($"DEBUG - Failed opening projector connection");

                return false;
            }
            catch (Exception)
            {
                Console.WriteLine($"DEBUG - Failed opening projector connection");
                return false;
            }
        }

        private void CloseConnection()
        {
            Console.WriteLine($"DEBUG - Closing projector connection");

            if (_client != null)
            {
                _client.Close();
            }
            if (_stream != null)
            {
                _stream.Close();
            }

            Console.WriteLine($"DEBUG - Finished closing projector connection");
        }

        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public bool TurnOn()
        {
            var turnOnCommand = new PowerCommand(PowerInstructionType.ON);
            return SendCommand(turnOnCommand) == ResponseType.SUCCESSFUL_EXECUTION;
        }

        public bool TurnOff()
        {
            var turnOffCommand = new PowerCommand(PowerInstructionType.OFF);
            return SendCommand(turnOffCommand) == ResponseType.SUCCESSFUL_EXECUTION;
        }

        public bool PowerQuery()
        {
            var powerQueryCommand = new PowerCommand(PowerInstructionType.QUERY);
            return SendCommand(powerQueryCommand) == ResponseType.SUCCESSFUL_EXECUTION;
        }

        public bool SetSource(InputInstructionType command)
        {
            var setInputCommand = new InputCommand(command);
            return SendCommand(setInputCommand) == ResponseType.SUCCESSFUL_EXECUTION;
        }

        public bool SourceQuery()
        {
            var sourceQueryCommand = new InputCommand(InputInstructionType.QUERY);
            return SendCommand(sourceQueryCommand) == ResponseType.SUCCESSFUL_EXECUTION;
        }
    }
}
