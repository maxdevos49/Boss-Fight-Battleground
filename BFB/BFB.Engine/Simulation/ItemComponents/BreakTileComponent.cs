using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;
using BFB.Engine.Simulation.SimulationComponents.Physics;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.ItemComponents
{
    public class BreakTileComponent : IItemComponent
    {

        private int _breakSpeed;
        
        public BreakTileComponent(int breakSpeed = 1)
        {
            _breakSpeed = breakSpeed;
        }
        
        public void Use(Simulation simulation, SimulationEntity entity, IItem item)
        {
            if (entity.ControlState == null)
                return;
            
            int mouseX = (int)entity.ControlState.Mouse.X;
            int mouseY = (int)entity.ControlState.Mouse.Y;

            Tuple<int, int, int, int> chunkInformation = simulation.World.TranslatePixelPosition(mouseX, mouseY);

            //If we are inside map
            if (chunkInformation == null) return;
            
            Chunk targetChunk = simulation.World.ChunkFromChunkLocation(chunkInformation.Item1, chunkInformation.Item2);
            
            //If the chunk exist
            if (targetChunk == null) return;
            
            int xSelection = chunkInformation.Item3;
            int ySelection = chunkInformation.Item4;

            if (item.TileTarget.X != xSelection || item.TileTarget.Y != ySelection)
            {
                item.TileTarget.X = xSelection;
                item.TileTarget.Y = ySelection;
                item.TileTarget.Progress = 0;
                return;
            }
            item.TileTarget.Progress++;

            bool isBlock;
            WorldTile tileType;
            ConfigurationRegistry config = ConfigurationRegistry.GetInstance();
            
            if ((tileType = (WorldTile)targetChunk.Block[xSelection,ySelection]) != 0)
            {//Is block
                isBlock = true;
                if (item.TileTarget.Progress < config.GetBlockConfiguration(tileType).BreakSpeed)
                    return;//Not ready yet

            }else if ((tileType = (WorldTile)targetChunk.Wall[xSelection, ySelection]) != 0)
            {//is wall
                isBlock = false;
                if (item.TileTarget.Progress < config.GetWallConfiguration(tileType).BreakSpeed)
                    return;//not ready yet
            }
            else
            {//If we get here then it means there is no block or wall
                return;
            }
            
            //If we made it here then we are ready to break a block
            targetChunk.ApplyBlockUpdate(new TileUpdate
            {
                X = (byte) xSelection,
                Y = (byte) ySelection,
                Mode = isBlock,
                TileValue = (ushort) WorldTile.Air
            });
            
            item.TileTarget.Progress = 0;
            
            int blockX = (simulation.World.WorldOptions.ChunkSize * chunkInformation.Item1 + xSelection) * simulation.World.WorldOptions.WorldScale;
            int blockY = (simulation.World.WorldOptions.ChunkSize * chunkInformation.Item2 + ySelection) * simulation.World.WorldOptions.WorldScale;
                
            InventoryManager inventory = new InventoryManager();
            Item newItem;
            if(isBlock)
                newItem = new Item(config.GetBlockConfiguration(tileType).ItemKey);
            else
                newItem = new Item(config.GetWallConfiguration(tileType).ItemKey);
            
            inventory.Insert(newItem);
            
            simulation.AddEntity(new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions
                {
                    TextureKey = config.GetBlockConfiguration(tileType).TextureKey,
                    Position = new BfbVector(blockX, blockY),
                    Dimensions = new BfbVector(1 * simulation.World.WorldOptions.WorldScale,
                        1 * simulation.World.WorldOptions.WorldScale),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Item
                }, new List<SimulationComponent>
                {
                    new LifetimeComponent(2000),
                    new TilePhysics()
                })
            {
                CollideFilter = "item",
                CollideWithFilters = new List<string>{ "tile" },
                Inventory = inventory
            });
                    
        }
    }
}