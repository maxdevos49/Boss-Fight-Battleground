using System;

namespace BFB.Engine.TileMap.Generators
{
    public class FlatWorld : WorldGenerator
    {
        private readonly Random _random;

        /**
         * Generates a flat world
         */
        public FlatWorld(WorldOptions options) : base(options)
        {
            _random = new Random(WorldOptions.Seed);
        }

        /**
         * Generates the chunk at the given x,y block position
         */
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
                        chunk.Block[xBlock,yBlock] = (int)WorldTile.Air;
                    }
                    else if (yActual < 17)
                    {
                        Console.WriteLine(_random.Next(10));
                        if(_random.Next(10) == 0)
                            chunk.Block[xBlock,yBlock] = (int)WorldTile.Grass;
                        else
                            chunk.Block[xBlock,yBlock] = (int)WorldTile.Dirt;
                    }
                    else if(yActual < 35)
                    {
                        chunk.Block[xBlock,yBlock] =
                            _random.Next(yActual) + 4 > 22 ? (ushort)WorldTile.Stone : (ushort)WorldTile.Dirt;
                    }
                    else
                    {
                        chunk.Block[xBlock,yBlock] = (int)WorldTile.Stone;
                    }
                }
            }
            
            return chunk;
        }
    }
}