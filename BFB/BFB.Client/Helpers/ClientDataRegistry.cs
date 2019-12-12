using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using JetBrains.Annotations;

namespace BFB.Client.Helpers
{
    public class ClientDataRegistry
    {
        public static ClientSettings Settings { get; set; }
        
        public static ConnectionSettings ConnectionSettings { get; set; }
        
        public bool GameReady { get; set; }
        
        [CanBeNull]
        public ClientEntity Client { get; set; }

        public Dictionary<string, ClientEntity> Entities;
        
        public WorldManager World { get; set; }

        public ClientInventory Inventory { get; set; }

        #region GetInstance
        
        private static ClientDataRegistry _instance;
        
        public static ClientDataRegistry GetInstance()
        {
            return _instance ?? (_instance = new ClientDataRegistry());
        }
        
        #endregion
       
        #region ClearInstance

        public void ClearInstance()
        {
            GameReady = false;
            Client = null;
            Entities.Clear();
            Entities = null;
            World = null;
            Inventory = null;
            
            _instance = null;
        }
        
        #endregion
        
        private ClientDataRegistry()
        {
            GameReady = false;
            World = new WorldManager(new WorldOptions
            {
                ChunkSize = 16,
                WorldChunkWidth = 1,
                WorldChunkHeight = 1,
                WorldScale = 30,
                WorldGenerator = options => new RemoteWorld(options)
            });
            
            Entities = new Dictionary<string, ClientEntity>();
            Inventory = new ClientInventory();
        }
    }
}