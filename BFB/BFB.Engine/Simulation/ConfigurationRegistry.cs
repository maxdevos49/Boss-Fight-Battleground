using System;
using System.Collections.Generic;
using System.IO;
using BFB.Engine.Content;
using BFB.Engine.Inventory;
using BFB.Engine.Inventory.Configuration;
using BFB.Engine.Simulation.ItemComponents;
using BFB.Engine.Simulation.SimulationComponents;
using BFB.Engine.TileMap;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace BFB.Engine.Simulation
{
    
    /// <summary>
    /// Holds a lot of configuration data read from JSON for use every where basically.
    /// Prevents needing a unique class for lots of things or inefficiently duplicating
    /// instances of a class when it could be used as a singleton
    /// </summary>
    public class ConfigurationRegistry : IConfigurationRegistry
    {
        private static ConfigurationRegistry _configuration;

        private static Dictionary<string, ItemConfiguration> _itemConfiguration;

        private static Dictionary<WorldTile, BlockConfiguration> _blockConfiguration;
        
        private static Dictionary<WorldTile, WallConfiguration> _wallConfiguration;

        #region Constructor
        
        private ConfigurationRegistry()
        {
            _itemConfiguration = new Dictionary<string, ItemConfiguration>();
            _blockConfiguration = new Dictionary<WorldTile, BlockConfiguration>();
            _wallConfiguration = new Dictionary<WorldTile, WallConfiguration>();
            
            ParseItems();          
        }
        
        #endregion

        #region IntializeRegistry
        
        [UsedImplicitly]
        public static void InitializeRegistry()
        {
            _configuration = new ConfigurationRegistry();
        }
        
        #endregion

        #region GetInstance
        
        public static ConfigurationRegistry GetInstance()
        {
            if(_configuration == null)
                InitializeRegistry();
            
            return _configuration;
        }
        
        #endregion
        

        #region ParseItemAndTileConfiguration
        
        private void ParseItems()
        {
            string json;
            
            using (StreamReader r = new StreamReader("Item.json"))
            {
                json = r.ReadToEnd();
            }
            
            ItemJSONSchema content = JsonConvert.DeserializeObject<ItemJSONSchema>(json);

            _itemConfiguration = content.Items;
            _blockConfiguration = content.Blocks;
            _wallConfiguration = content.Walls;
        }
        
        #endregion
        
        #region GetItemConfiguration

        public ItemConfiguration GetItemConfiguration(string itemKey)
        {
            return _itemConfiguration.ContainsKey(itemKey) ? _itemConfiguration[itemKey] : null;
        }
        
        #endregion

        #region GetBlockConfiguration
        
        public BlockConfiguration GetBlockConfiguration(WorldTile tileKey)
        {
            return _blockConfiguration.ContainsKey(tileKey) ? _blockConfiguration[tileKey] : null;
        }
        
        #endregion
        
        #region GetWallConfiguration

        public WallConfiguration GetWallConfiguration(WorldTile wallKey)
        {
            return _wallConfiguration.ContainsKey(wallKey) ? _wallConfiguration[wallKey] : null;
        }
        
        #endregion
        
        
        #region ParseItemComponents(TODO)
        
        #endregion

        #region GetItemComponent(TODO)
        
        public IItemComponent GetItemComponent(string componentKey)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region GetSimulationComponent(TODO)
        
        public ISimulationComponent GetSimulationComponent(string componentKey)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        #region GetSingletonSimulationComponent(TODO)
        public ISimulationComponent GetSingletonSimulationComponent(string componentKey)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        
        #region GetInstance
        
        private object GetInstance(string strFullyQualifiedName)
        {         
            Type t = Type.GetType(strFullyQualifiedName); 
            
            if(t == null)
                throw new Exception($"The fully qualified name: \"{strFullyQualifiedName}\" was not valid.");
            
            return  Activator.CreateInstance(t);         
        }
        
        #endregion
    }
}