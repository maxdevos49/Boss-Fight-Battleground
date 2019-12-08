using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.TileMap;
using JetBrains.Annotations;

namespace BFB.Engine.Helpers
{
    public static class WorldExtensions
    {
        [UsedImplicitly]
        public static List<Chunk> QueryChunks(this WorldManager world, Rectangle selection, Action<Chunk> action = null)
        {
            int xStart = selection.Left / world.ChunkPixelSize();
            int xEnd = selection.Right / world.ChunkPixelSize();

            int yStart = selection.Top / world.ChunkPixelSize();
            int yEnd = selection.Bottom / world.ChunkPixelSize();

            if (xStart < 0)
                xStart = 0;
            if (xEnd < 0)
                xEnd = 0;

            if (xStart>= world.WorldOptions.WorldChunkWidth)
                xStart = world.WorldOptions.WorldChunkWidth - 1;
            
            if (xEnd >= world.WorldOptions.WorldChunkWidth)
                xEnd = world.WorldOptions.WorldChunkWidth - 1;
            
            if (yStart < 0)
                yStart = 0;
            if (yEnd < 0)
                yEnd = 0;

            if (yStart>= world.WorldOptions.WorldChunkHeight)
                yStart = world.WorldOptions.WorldChunkHeight - 1;
            
            if (yEnd >= world.WorldOptions.WorldChunkHeight)
                yEnd = world.WorldOptions.WorldChunkHeight - 1;

            Dictionary<string,Chunk> chunks = new Dictionary<string, Chunk>();

            for (int y = yStart; y <= yEnd; y++)
                for (int x = xStart; x <= xEnd; x++)
                    if (!chunks.ContainsKey(world.ChunkMap[x, y].ChunkKey)) //Prevent duplicating chunk insertions
                    {
                        chunks.Add(world.ChunkMap[x, y].ChunkKey,world.ChunkMap[x, y]);
                        action?.Invoke(world.ChunkMap[x, y]);
                    }

            return chunks.Values.ToList();
        }
        
        [UsedImplicitly]
        public static List<SimulationEntity> QueryEntities(this WorldManager world, Rectangle selection, List<string> filter, string excludeKey = null)
        {
            List<SimulationEntity> entities = new List<SimulationEntity>();

            world.QueryChunks(selection, chunk =>
            {
//                if (filter == null)
                    entities.AddRange(chunk.Entities.Values.Where(x => 
                                                        x.EntityId != excludeKey
                                                        && Collision.IsRectangleColliding(x.Bounds, selection)));
//                else
//                    entities.AddRange(chunk.Entities.Values.Where(x =>
//                                                        filter.Contains(x.CollideFilter) 
//                                                        && x.EntityId != excludeKey 
//                                                        && Collision.IsRectangleColliding(x.Bounds, selection)));
            });

            return entities;
        }
    }
}