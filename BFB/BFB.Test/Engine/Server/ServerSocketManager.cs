using System;
using System.Net;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using Xunit;

namespace BFB.Test.Engine.Server
{
    public class ServerSocketManagerTest
    {

        [Fact]
        public void OnClientAuthentication()
        {
            ServerSocketManager s = new ServerSocketManager(IPAddress.Parse("127.0.0.1"), 6969);
            
            Assert.IsType<Func<DataMessage,bool>>(s.OnClientAuthentication);
            Assert.NotNull(s.OnClientAuthentication);
            s.Dispose();
        }
        
        [Fact]
        public void OnClientConnect()
        {
            ServerSocketManager s = new ServerSocketManager(IPAddress.Parse("127.0.0.1"), 6969);
            
            Assert.Null(s.OnClientConnect);
            s.OnClientConnect = socket => { }; 
            Assert.IsType<Action<ClientSocket>>(s.OnClientConnect);
            s.Dispose();
        }
        
        [Fact]
        public void OnClientReady()
        {
            ServerSocketManager s = new ServerSocketManager(IPAddress.Parse("127.0.0.1"), 6969);
            
            Assert.Null(s.OnClientReady);
            s.OnClientReady = socket => { }; 
            Assert.IsType<Action<ClientSocket>>(s.OnClientReady);
            s.Dispose();
        }
        
        [Fact]
        public void OnClientDisconnect()
        {
            ServerSocketManager s = new ServerSocketManager(IPAddress.Parse("127.0.0.1"), 6969);
            
            Assert.Null(s.OnClientDisconnect);
            s.OnClientDisconnect = s1 => {  }; 
            Assert.IsType<Action<string>>(s.OnClientDisconnect);
            s.Dispose();
        }
        
        [Fact]
        public void OnServerStart()
        {
            ServerSocketManager s = new ServerSocketManager(IPAddress.Parse("127.0.0.1"), 6969);
            
            Assert.Null(s.OnServerStart);
            s.OnServerStart = () => { };
            Assert.IsType<Action>(s.OnServerStart);
            s.Dispose();
        }
        
        [Fact]
        public void OnServerStop()
        {
            ServerSocketManager s = new ServerSocketManager(IPAddress.Parse("127.0.0.1"), 6969);
            
            Assert.Null(s.OnServerStop);
            s.OnServerStop = () => { };
            Assert.IsType<Action>(s.OnServerStop);
            s.Dispose();
        }
    }
}