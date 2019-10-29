using System.Collections.Generic;

namespace BFB.Engine.TileMap
{
    public class ChunkUpdate
    {
        public string ChunkKey { get; set; }
        
        public List<TileUpdate> TileChanges { get; set; }
    }

    public class TileUpdate
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        //True = wall, False = block
        public bool Mode { get; set; }

        public int TileValue { get; set; }
        
    }
}