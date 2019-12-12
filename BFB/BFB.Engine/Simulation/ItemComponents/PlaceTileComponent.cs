using System;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.ItemComponents
{
    public class PlaceTileComponent : IItemComponent
    {
        public void Use(Simulation simulation, SimulationEntity entity, IItem item)
        {
            if (entity.ControlState == null || entity.Inventory == null || !(item.Configuration.ItemType == ItemType.Block || item.Configuration.ItemType == ItemType.Wall))
                return;
            
            int mouseX = (int)entity.ControlState.Mouse.X;
            int mouseY = (int)entity.ControlState.Mouse.Y;

            Tuple<int, int, int, int> chunkInformation = simulation.World.TranslatePixelPosition(mouseX, mouseY);

            //If we are inside map
            if (chunkInformation == null) return;
            
            Chunk targetChunk = simulation.World.ChunkFromChunkLocation(chunkInformation.Item1, chunkInformation.Item2);

            int xSelection = chunkInformation.Item3;
            int ySelection = chunkInformation.Item4;

            //If the chunk exist
            if (targetChunk == null) return;

            //we cant place a wall or block if its covered
            if (targetChunk.Block[xSelection, ySelection] != 0)
                return;
            
            //only place if no block exist at location
            if (item.Configuration.ItemType == ItemType.Block)
            {//place block
                targetChunk.ApplyBlockUpdate(new TileUpdate
                {
                    X = (byte) xSelection,
                    Y = (byte) ySelection,
                    Mode = true,
                    TileValue = (ushort) item.Configuration.TileKey
                });
            }
            else if (targetChunk.Wall[xSelection, ySelection] == 0)
            {//place wall
                targetChunk.ApplyBlockUpdate(new TileUpdate
                {
                    X = (byte) xSelection,
                    Y = (byte) ySelection,
                    Mode = false,
                    TileValue = (ushort) item.Configuration.TileKey
                });
                
            }
            else
            {
                return;
            }

            if (item.DecrementStack()) return;
            
            byte slotId = entity.Inventory.GetActiveSlotId();
            entity.Inventory.Remove(slotId);

        }
    }
}