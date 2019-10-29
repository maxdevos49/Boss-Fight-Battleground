using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.TileMap.Generators;
using JetBrains.Annotations;

namespace BFB.Engine.TileMap
{
    public class Chunk
    {
        #region Properties
        
        public readonly string ChunkId;
        public readonly int ChunkX;
        public readonly int ChunkY;
        
        public readonly int ChunkSize;
        public ushort[,] Hardness { get; }
        public byte[,] Light { get; }
        public int[,] Wall { get;}
        public  int[,] Block { get; }
        
//        public Dictionary<string,ServerEntity> Entities { get; }

        #endregion

        #region Constructor

        public Chunk(int chunkSize, int chunkX, int chunkY)
        {
            //Chunk Meta data
            ChunkId = Guid.NewGuid().ToString();
            ChunkSize = chunkSize;
            ChunkX = chunkX/ChunkSize;
            ChunkY = chunkY/ChunkSize;
            
            //Tile information
            Hardness = new ushort[ChunkSize, ChunkSize];
            Light = new byte[ChunkSize, ChunkSize];
            
            Wall = new int[ChunkSize, ChunkSize];
            Block = new int[ChunkSize, ChunkSize];
            
            //Entities
//            Entities = new Dictionary<string, ServerEntity>();
        }
        
        #endregion

    }
}
