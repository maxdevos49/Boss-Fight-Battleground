
using System.Net.Sockets;

namespace BFBEngine.Socket
{
    public class SocketClient
    {

        private readonly int Port;

        private readonly string IpAddress;

        private bool Connected;

        private TcpClient TcpClient;

        private NetworkStream Stream;

        public SocketClient(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
            Connected = false;
        }

        public void Connect()
        {
            if (!Connected)
            {
                TcpClient = new TcpClient(IpAddress, Port);
                Stream = TcpClient.GetStream();
                Connected = true;
            }
        }

        public void Disconnect()
        {
            if (Connected)
            {
                Stream.Close();
                TcpClient.Close();
            }
        }

        public bool IsConnected()
        {
            return false;
        }


    }
}
