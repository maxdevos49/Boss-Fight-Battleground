using System;

namespace BFB.Engine.TileMap.TileComponent
{
    public class GrassTileComponent : ITileComponent
    {
        public void TickTile(Chunk chunk, int blockX, int blockY)
        {
            //TODO
            Console.WriteLine("Grass Tick");
        }
    }
}