using System;

namespace BFB.Engine.TileMap.Generators
{
    /// <summary>
    /// Used to generate a flat world.
    /// </summary>
    public class FlatWorld : WorldGenerator
    {
        private readonly Random _random;
        
        /// <summary>
        /// Generates a flat world.
        /// </summary>
        /// <param name="options">Options used to generate the world.</param>
        public FlatWorld(WorldOptions options) : base(options)
        {
            _random = new Random(WorldOptions.Seed);
        }
        
        /// <summary>
        /// Generates the chunk at the given x,y block position.
        /// </summary>
        /// <param name="x">The chunk X location with the given block X location.</param>
        /// <param name="y">The chunk Y location with the given block Y location.</param>
        /// <returns>Returns the generated chuck.</returns>
        public override Chunk GenerateChunk(int x, int y)
        {
            Chunk chunk = new Chunk(WorldOptions.ChunkSize, x, y);

            for (int yBlock = 0; yBlock < chunk.ChunkSize; yBlock++)
            {
                for (int xBlock = 0; xBlock < chunk.ChunkSize; xBlock++)
                {

                    int yActual = y + yBlock;
                    
                    if(yActual < 16)
                    {
                        chunk.Block[xBlock,yBlock] = (ushort)WorldTile.Air;
                    }
                    else if (yActual < 17)
                    {
//                        Console.WriteLine(_random.Next(10));
                        if(_random.Next(10) == 0)
                            chunk.Block[xBlock,yBlock] = (ushort)WorldTile.Grass;
                        else
                        {
                            chunk.Block[xBlock, yBlock] = (ushort) WorldTile.Dirt;
                            chunk.Wall[xBlock, yBlock] = (ushort) WorldTile.Dirt;
                        }

                    }
                    else if(yActual < 35)
                    {
                        if (_random.Next(yActual) + 4 > 22)
                        {
                            chunk.Block[xBlock, yBlock] = (ushort) WorldTile.Stone;
                            chunk.Wall[xBlock, yBlock] = (ushort) WorldTile.Stone;
                        }
                        else
                        {
                            chunk.Block[xBlock,yBlock]= (ushort)WorldTile.Dirt;
                            chunk.Wall[xBlock, yBlock] = (ushort) WorldTile.Dirt;
                        }
                        
                    }
                    else
                    {
                        chunk.Block[xBlock,yBlock] = (ushort)WorldTile.Stone;
                        chunk.Wall[xBlock, yBlock] = (ushort) WorldTile.Stone;
                    }
                }
            }
            
            return chunk;
        }
    }
}