using System;
using System.Net.Sockets;
using System.Threading;

class Program
{

    static void Main(string[] args)
    {
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Connect("10.27.134.6", "Hello I'm Device 1...");
        }).Start();

        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Connect("10.27.134.6", "Hello I'm Device 2...");
        }).Start();


        Console.ReadLine();
    }

    static void Connect(String server, String message)
    {
        try
        {
            Int32 port = 8080;
            TcpClient client = new TcpClient(server, port);

            NetworkStream stream = client.GetStream();

            int count = 0;
            while (count++ < 1000)
            {
                // Translate the Message into ASCII.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);

                // Bytes Array to receive Server Response.
                data = new Byte[256];
                String response = String.Empty;

                // Read the Tcp Server Response Bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", response);

                Thread.Sleep(2000);
            }

            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
        }

        Console.Read();
    }
}