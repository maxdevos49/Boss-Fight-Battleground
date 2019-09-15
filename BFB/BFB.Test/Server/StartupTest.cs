using System;
using Xunit;

using BFB.Server;

namespace BFB.Test.Server
{
	public class StartupTest
	{
        
        private Startup Startup { get; set; }

		public StartupTest() {
            Startup = new Startup();
        }

        [Fact]
        public void Configuration_Exist()
        {
            Assert.NotNull(Startup.Configuration);
        }

        [Fact]
        public void Port_Exist()
        {
            Assert.NotEmpty(Startup.Configuration.GetSection("Server")["Port"]);
        }

        [Fact]
        public void IPAddress_Exist()
        {
            Assert.NotEmpty(Startup.Configuration.GetSection("Server")["IPAddress"]);
        }

    }
}
