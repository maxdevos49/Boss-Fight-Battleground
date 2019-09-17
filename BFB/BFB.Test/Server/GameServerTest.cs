using System.Net.Sockets;
using BFB.Engine.Server;
using Xunit;

namespace BFB.Test.Server
{
    public class GameServerTest
    {

        [Fact]
        public void Connection()
        {

            TcpClient client = new TcpClient("10.31.97.237", 6969);

            var message = new Packet
            {
                Route = "Server Test",
                Data = new byte[100]
            };

            var stream = client.GetStream();

            PacketManager.Write(stream, message);

            Assert.True(client.Connected);
            client.Close();
        }

        [Fact]
        public void Connection2()
        {
            TcpClient client = new TcpClient("127.0.0.1", 6969);
            Assert.True(client.Connected);
            client.Close();
        }
    }
}
