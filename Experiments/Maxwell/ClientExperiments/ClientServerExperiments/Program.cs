using System;
using System.Threading;

namespace ClientServerExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                Server myserver = new Server("10.27.134.6", 8080);
            });

            t.Start();

            Console.WriteLine("Server Started...!");
            
        }
    }
}
