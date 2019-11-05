using System;
using System.Collections.Generic;
using BFB.Engine.TileMap;

namespace BFB.Engine.Server.Communication
{
    /// <summary>
    /// Used to pass ChunkUpdates and ChunkTileUpdates to the client
    /// </summary>
    [Serializable]
    public class ChunkUpdatesMessage : DataMessage
    {
        public ChunkUpdatesMessage()
        {
            ChunkUpdates = new List<ChunkUpdate>();
            ChunkTileUpdates = new List<ChunkTileUpdates>();
        }
        
        /// <summary>
        /// Holds Chunk Updates for updating an entire chunk
        /// </summary>
        public List<ChunkUpdate> ChunkUpdates { get; }

        /// <summary>
        /// Holds Chunk Tile Updates that update individual tiles
        /// </summary>
        public List<ChunkTileUpdates> ChunkTileUpdates { get; }
    }
}