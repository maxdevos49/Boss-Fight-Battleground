using System.Net.Sockets;

namespace BFB.Server.Client
{
    public class Client
    {
        public string Name { get; set; }

        public ClientStatus Status { get; set; }

        public TcpClient Socket { get; set; }

        public void Send()
        {
            //send stuff here
        }
    }
}
