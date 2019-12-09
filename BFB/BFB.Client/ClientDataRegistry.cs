using System.Collections.Generic;
using System.ComponentModel;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using JetBrains.Annotations;

namespace BFB.Client
{
    public class ClientDataRegistry
    {
        [UsedImplicitly]
        public static string Ip { get; set; }
        
        [UsedImplicitly]
        public static int Port { get; set; }
        
        [UsedImplicitly]
        public bool GameReady { get; set; }
        
        [UsedImplicitly]
        [CanBeNull]
        public ClientEntity Client { get; set; }

        [UsedImplicitly]
        public Dictionary<string, ClientEntity> Entities;
        
        [UsedImplicitly]
        public WorldManager World { get; set; }

        [UsedImplicitly] 
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