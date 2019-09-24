using System;
using System.Net.Sockets;
using BFB.Engine.Server.Communication;
using BFB.Engine.Server.Socket;
using Xunit;
using Xunit.Abstractions;

namespace BFB.Test.Engine.Server
{
    public class ServerSocketManager
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ServerSocketManager(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Connection()
        {
            
        }
    }
}