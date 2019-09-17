using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BFB.Server.Client
{
    public class ClientManager
    {

        private object _lock = new object();

        private int NextClientId;
        private Dictionary<int, TcpClient> Clients;

        public ClientManager()
        {
            NextClientId = 0;
            Clients = new Dictionary<int, TcpClient>();//Will later on have a custom client class that holds more information maybe??
        }

        public int Add(TcpClient client)
        {
            lock (_lock)
            {
                NextClientId++;
                Clients.Add(NextClientId, client);
                return NextClientId;
            }
        }

        public bool Remove(int clientKey)
        {
            lock (_lock)
            {
                return Clients.Remove(clientKey);
            }
        }

        public TcpClient Get(int clientId)
        {
            lock (_lock)
            {
                return Clients[clientId];
            }
        }

        public List<TcpClient> GetAll()
        {
            lock (_lock)
            {
                return Clients.Values.ToList();
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return Clients.Count;
            }
        }

    }
}
