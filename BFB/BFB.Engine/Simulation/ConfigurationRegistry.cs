using System;
using System.Collections.Generic;
using System.IO;
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

        private Dictionary<string, ItemConfiguration> _itemConfiguration;

        private Dictionary<WorldTile, BlockConfiguration> _blockConfiguration;
        
        private Dictionary<WorldTile, WallConfiguration> _wallConfiguration;

        private readonly Dictionary<string, IItemComponent> _itemComponents;

        #region Constructor
        
        private ConfigurationRegistry()
        {
            _itemConfiguration = new Dictionary<string, ItemConfiguration>();
            _blockConfiguration = new Dictionary<WorldTile, BlockConfiguration>();
            _wallConfiguration = new Dictionary<WorldTile, WallConfiguration>();
            
            _itemComponents = new Dictionary<string, IItemComponent>();

            
            ParseItems();

            ParseItemComponents();
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
        
        
        #region ParseItemComponents

        private void ParseItemComponents()
        {
            _itemComponents.Add("PlaceBlock", new PlaceTileComponent());
            _itemComponents.Add("BreakBlock", new BreakBlockComponent());
            _itemComponents.Add("BreakWall", new BreakWallComponent());
            _itemComponents.Add("Hit", new HitComponent());
            _itemComponents.Add("CastSpell", new CastSpell());
        }
        
        #endregion

        #region GetItemComponent
        
        public IItemComponent GetItemComponent(string componentKey)
        {
            if(!_itemComponents.ContainsKey(componentKey))
                throw new KeyNotFoundException($"The Item Component with the Key: {componentKey} was not found");

            return _itemComponents[componentKey];
        }
        
        #endregion

        #region GetSimulationComponent(TODO)
        
        public EntityComponent GetSimulationComponent(string componentKey)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        #region GetSingletonSimulationComponent(TODO)
        public EntityComponent GetSingletonSimulationComponent(string componentKey)
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