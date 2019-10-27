using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

//https://codinginfinite.com/multi-threaded-tcp-server-core-example-csharp/

class Server
{
    readonly TcpListener server = null;

    public Server(string ip, int port)
    {
        //Parse Ip Address
        IPAddress localAddr = IPAddress.Parse(ip);

        //Create tcp server at address and port
        server = new TcpListener(localAddr, port);

        //start the server
        server.Start();

        //call the listener thing
        StartListener();
    }

    public void StartListener()
    {

        try
        {
            while (true)
            {
                //Indicate waiting for connections
                Console.WriteLine("Waiting for a connection...");

                //Wait for a new connection
                TcpClient client = server.AcceptTcpClient();

                //Announce new connection attempt
                Console.WriteLine("Connected!");

                //Give client a new thread
                Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));

                //start the client threads
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
           
        }
    }

    public void HandleDeivce(object obj)
    {

        //cast client object because of how paramaterized threads work
        TcpClient client = (TcpClient)obj;

        //get the network stream for the client
        var stream = client.GetStream();

        string imei = String.Empty;

        string data = null;

        byte[] bytes = null;
        int i;

        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                //object = BitConverter.ToObject
                string hex = BitConverter.ToString(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);

                string str = "Hey Device!";
                Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                stream.Write(reply, 0, reply.Length);
                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }
    }
}