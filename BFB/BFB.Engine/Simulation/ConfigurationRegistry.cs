using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BFB.Engine.Entity.Configuration;
using BFB.Engine.Inventory.Configuration;
using BFB.Engine.Simulation.BlockComponent;
using BFB.Engine.Simulation.Configuration;
using BFB.Engine.Simulation.EntityComponents;
using BFB.Engine.Simulation.ItemComponents;
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

        private Dictionary<string, EntityConfiguration> _entityConfigurations;
        
        private Dictionary<string, EntityComponentConfiguration> _entityComponentConfigurations;
        
        private readonly Dictionary<string, IItemComponent> _itemComponents;

        #region Constructor
        
        private ConfigurationRegistry()
        {
            _itemConfiguration = new Dictionary<string, ItemConfiguration>();
            _blockConfiguration = new Dictionary<WorldTile, BlockConfiguration>();
            _wallConfiguration = new Dictionary<WorldTile, WallConfiguration>();
            _entityConfigurations = new Dictionary<string, EntityConfiguration>();
            _entityComponentConfigurations = new Dictionary<string, EntityComponentConfiguration>();
            _itemComponents = new Dictionary<string, IItemComponent>();

            ParseItems();
            ParseEntities();
            
            ParseComponents();
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
        
        
        #region ParseEntityConfiguration

        private void ParseEntities()
        {
            string json;
            
            using (StreamReader r = new StreamReader("Entity.json"))
            {
                json = r.ReadToEnd();
            }
            
            EntityJSONSchema content = JsonConvert.DeserializeObject<EntityJSONSchema>(json);

            _entityConfigurations = content.Entities;

        }
        
        #endregion

        #region GetEntityConfiguration
        
        public EntityConfiguration GetEntityConfiguration(string entityKey)
        {
            if(!_entityConfigurations.ContainsKey(entityKey))
                throw new KeyNotFoundException($"The Entity with the Key: {entityKey} was not found");

            return _entityConfigurations[entityKey].Clone();
        }
        
        #endregion
        
        
        #region ParseConponents

        private void ParseComponents()
        {
            string json;
            
            using (StreamReader r = new StreamReader("Component.json"))
            {
                json = r.ReadToEnd();
            }
            
            ComponentJSONSchema content = JsonConvert.DeserializeObject<ComponentJSONSchema>(json);

            //Entity Components
            _entityComponentConfigurations = content.EntityComponents;
            
            //Item components
            ParseItemComponents();
            
            //block components
            //TODO move from the current block component
        }

        #endregion
        
        #region GetEntityComponent

        public EntityComponent GetEntityComponent(string componentKey)
        {
            if(!_entityComponentConfigurations.ContainsKey(componentKey))
                throw new KeyNotFoundException($"The Component with the Key: {componentKey} was not found");

            EntityComponentConfiguration config =  _entityComponentConfigurations[componentKey];

            return (EntityComponent)GetEntityComponentInstance(config);
        }

        #endregion
        
        
        #region ParseItemComponents

        private void ParseItemComponents()
        {
            //TODO use json configuration
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
        
        
        #region ParseBlockComponents

        private void ParseBlockComponents()
        {
            //TODO move from other manager
        }

        #endregion
        
        #region GetBlockComponent

        public ITileComponent GetBlockComponent()
        {
            //TODO
            return null;
        }
        
        #endregion
        

        #region GetEntityComponentInstance
        
        private object GetEntityComponentInstance(EntityComponentConfiguration config)
        {
            try
            {
                Type t = Type.GetType(config.FullyQualifiedName);

                if (t == null)
                    throw new Exception($"The fully qualified name: \"{config.FullyQualifiedName}\" was not valid.");

                if (config.Args == null) 
                    return Activator.CreateInstance(t);
                
                //We want to speak in int32's and not int64's
                for (int i = 0; i < config.Args.Length; i++)
                {
                    if (config.Args[i] is long)
                        config.Args[i] = Convert.ToInt32(config.Args[i]);
                }

                return Activator.CreateInstance(t, config.Args);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(config.FullyQualifiedName);
                Console.WriteLine();
                Console.WriteLine($"Exception: {ex}");
                return null;
            }
        }
        
        #endregion
    }
}