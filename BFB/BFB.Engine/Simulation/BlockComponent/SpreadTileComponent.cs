using System;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.BlockComponent
{
    /// <summary>
    /// Used to have a tile spread in the tilemap.
    /// </summary>
    public class SpreadTileComponent : ITileComponent
    {
        private readonly Random _random;
        private readonly byte _randomness;
        
        private readonly WorldTile _spreadBlock;
        private readonly WorldTile _targetBlock;
        private readonly WorldTile _aboveBlock;

        /// <summary>
        /// Constructs a spread tile component with a given configuration.
        /// </summary>
        /// <param name="spreadBlock">The block that is placed while spreading.</param>
        /// <param name="targetBlock">The block that will change configuration to the given spread block.</param>
        /// <param name="aboveBlock">The block above the target block.</param>
        /// <param name="randomness">How often the spreadBlock spreads.</param>
        public SpreadTileComponent(WorldTile spreadBlock, WorldTile targetBlock, WorldTile aboveBlock = WorldTile.Unknown, byte randomness = 100)
        {
            _random = new Random();
            _randomness = randomness;

            _spreadBlock = spreadBlock;
            _targetBlock = targetBlock;
            _aboveBlock = aboveBlock;
        }
        
        /// <summary>
        /// The update for the tile component.
        /// </summary>
        /// <param name="world">The manager for the world.</param>
        /// <param name="chunk">The chunk in a given world.</param>
        /// <param name="blockX">The x block in the given chunk.</param>
        /// <param name="blockY">The y block in the given chunk.</param>
        public void TickTile(WorldManager world, Chunk chunk, int blockX, int blockY)
        {
            
            //randomly selected area around the grass block
            int xSelection = blockX + _random.Next(2) - 1;
            int ySelection = blockY + _random.Next(2) - 1;
            
            //Current chunk coordinates
            int chunkX = chunk.ChunkX;
            int chunkY = chunk.ChunkY;
            
            //Validate the selection location
            if (xSelection < 0 && chunkX <= 0
                || xSelection >= world.WorldOptions.ChunkSize && chunkX >= world.WorldOptions.WorldChunkWidth
                || ySelection < 0 && chunkY <= 0
                ||  ySelection >= world.WorldOptions.ChunkSize && chunkY >= world.WorldOptions.WorldChunkHeight) return;
                
            //get actual x chunk coordinates for target block
            if (xSelection < 0)
            {
                chunkX--;
                xSelection = world.WorldOptions.ChunkSize - 1;
            }
            else if (xSelection > world.WorldOptions.ChunkSize)
            {
                chunkX++;
                xSelection = 0;
            }

            //get actual y chunk coordinates for target block
            if (ySelection < 0)
            {
                chunkY--;
                ySelection = world.WorldOptions.ChunkSize - 1;
            }
            else if (ySelection > world.WorldOptions.ChunkSize)
            {
                chunkY++;
                ySelection = 0;
            }

            //only continue if the block around us is a dirt block
            if (world.ChunkMap[chunkX, chunkY].Block[xSelection,ySelection] != (ushort) _targetBlock)
                return;

            if (_aboveBlock != WorldTile.Unknown)
            {
                WorldTile isAboveBlock;

                //If less then 0 then we check chunk above
                if (ySelection - 1 < 0)
                    isAboveBlock = (WorldTile) world.ChunkMap[chunk.ChunkX, chunk.ChunkY - 1]
                        .Block[xSelection, world.WorldOptions.ChunkSize - 1];
                else
                    isAboveBlock = (WorldTile) chunk.Block[xSelection, ySelection - 1];

                if (isAboveBlock != _aboveBlock) return;
            }

            //Reduce grass growth speed
            if (_random.Next(256) > _randomness)
                return;
                
            chunk.ApplyBlockUpdate(new TileUpdate
            {
                Mode = true,
                TileValue = (ushort) _spreadBlock,
                X = (byte) xSelection,
                Y = (byte) ySelection
            });

        }
    }
}