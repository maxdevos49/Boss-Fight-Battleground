//C#
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace BFB.Server
{
    public class Startup
    {

        public readonly IConfigurationRoot Configuration;
        private GameServer Server;

        static void Main(string[] args)
        {
            var startup = new Startup();
            startup.Start();
        }

        public Startup()
        {
            //get configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            Configuration = builder.Build();
        }

        public void Start()
        {
            Thread t = new Thread(() => { Server = new GameServer(Configuration); Server.Start(); });
            t.Start();
        }

        public void Stop()
        {
            Server.Stop();
        }
    }
}
