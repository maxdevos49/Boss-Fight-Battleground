using System;
using System.Collections.Generic;
using BFB.Engine.TileMap;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class ChunkUpdatesMessage : DataMessage
    {
        public ChunkUpdatesMessage()
        {
            ChunkUpdates = new List<ChunkUpdate>();
            ChunkTileUpdates = new List<ChunkTileUpdates>();
        }
        
        public List<ChunkUpdate> ChunkUpdates { get; }

        public List<ChunkTileUpdates> ChunkTileUpdates { get; }
    }
}