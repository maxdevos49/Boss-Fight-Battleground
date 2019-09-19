using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BFB.Server.Client
{
    public class ClientManager
    {

        private readonly object _lock = new object();
        private readonly Dictionary<int, TcpClient> _clients;
        private int _nextClientId;

        public ClientManager()
        {
            _nextClientId = 0;
            _clients = new Dictionary<int, TcpClient>();//Will later on have a custom client class that holds more information maybe??
        }

        public int Add(TcpClient client)
        {
            lock (_lock)
            {
                _nextClientId++;
                _clients.Add(_nextClientId, client);
                return _nextClientId;
            }
        }

        public bool Remove(int clientKey)
        {
            lock (_lock)
            {
                return _clients.Remove(clientKey);
            }
        }

        public TcpClient Get(int clientId)
        {
            lock (_lock)
            {
                return _clients[clientId];
            }
        }

        public List<TcpClient> GetAll()
        {
            lock (_lock)
            {
                return _clients.Values.ToList();
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return _clients.Count;
            }
        }

    }
}
