using System;
using System.Collections.Generic;
using System.IO;
using BFB.Engine.Entity;
using BFB.Engine.TileMap.Generators;
using BFB.Engine.TileMap.IO;
using Newtonsoft.Json;

namespace BFB.Engine.TileMap
{

    public class WorldManager
    {
        
        #region Properties
        
        public WorldOptions WorldOptions { get; private set; }
        
        public Chunk[,] ChunkMap { get; private set; }
        
        public Dictionary<string, Chunk> ChunkIndex { get; }
        
        public Action<string> WorldGeneratorCallback { get; set; }
        
        
        private readonly WorldGenerator _worldGenerator;

        #endregion
        
        #region Constructor
        
        public WorldManager(WorldOptions worldOptions)
        {
            WorldOptions = worldOptions;
            _worldGenerator = worldOptions.GetWorldGenerator?.Invoke(WorldOptions);
            ChunkMap = new Chunk[WorldOptions.WorldChunkWidth, WorldOptions.WorldChunkHeight];
            ChunkIndex = new Dictionary<string, Chunk>();
        }
        
        #endregion

        //Move to world generator?
        #region GenerateWorld
        
        /**
         * Generates the entire map at once
         */
        public void GenerateWorld(Action<string> progressCallback)
        {
            if (_worldGenerator == null)
                return;
            
            int previousPercent = 0;
            float progress = 0;
            for (int y = 0; y < WorldOptions.WorldChunkHeight; y++)
            {
                for (int x = 0; x < WorldOptions.WorldChunkWidth; x++)
                {
                    ChunkMap[x, y] = _worldGenerator.GenerateChunk(x * WorldOptions.ChunkSize, y * WorldOptions.ChunkSize);

                    int currentPercent = (int)(progress++ / (WorldOptions.WorldChunkHeight * WorldOptions.WorldChunkWidth) * 100f);
                    
                    if (previousPercent == currentPercent) continue;
                    
                    progressCallback?.Invoke(currentPercent + "%");
                    previousPercent = currentPercent;
                }
            }
            
            MapChunksToIndex();
            
            progressCallback?.Invoke("100% - Generation Complete");
        }
        
        #endregion
        
        #region MapChunksToIndex

        /**
         * Simply adds each chunk to a dictionary so depending on if you have the x/y position or the chunk key then you always have O(1) lookup
         */
        private void MapChunksToIndex()
        {
            ChunkIndex.Clear();
            for (int y = 0; y < WorldOptions.WorldChunkHeight; y++)
            {
                for (int x = 0; x < WorldOptions.WorldChunkWidth; x++)
                {
                    ChunkIndex.Add(ChunkMap[x, y].ChunkKey, ChunkMap[x, y]);
                }
            }
        }
        
        #endregion
        
        //move to world IO?
        #region SaveWorld
        
        /**
         * Saves the current state of the world with a given in the world folder
         */
        public bool SaveWorld(string fileName)
        {
            WorldDataSchema worldData = new WorldDataSchema
            {
                WorldConfig = WorldOptions,
                Chunks = ChunkMap
            };

            string serializedWorldData = JsonConvert.SerializeObject(worldData, Formatting.None,
                new JsonSerializerSettings()
                { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            try
            {
                string path = Directory.GetCurrentDirectory() + "/Worlds/" + fileName + ".json";
                
                //Create directory. If exist it does nothing
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Worlds/");
                
                using (StreamWriter s = File.CreateText(path))
                {
                    s.Write(serializedWorldData);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        
        #endregion

        //move to world IO?
        #region LoadWorld
        
        public bool LoadWorld(string fileName)
        {
            //TODO not sure if works at all
            string path = Directory.GetCurrentDirectory() + "/Worlds/" + fileName + ".json";

            try
            {
                //Get file contents for Parsing
                string json;
                using (StreamReader r = new StreamReader(path))
                {
                    json = r.ReadToEnd();
                }

                WorldDataSchema worldData = JsonConvert.DeserializeObject<WorldDataSchema>(json);

                WorldOptions = worldData.WorldConfig;
                ChunkMap = worldData.Chunks;
                
                MapChunksToIndex();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("No world with filename: \"" + fileName + "\" exists.");
                return false;
            }

            
        }
        
        #endregion

        #region MoveEntity

        /**
         * Moves entity from one chunk to another
         */
        public void MoveEntity(string entityKey, Chunk originalChunk, Chunk newChunk)
        {
            if (!originalChunk.Entities.ContainsKey(entityKey))
                return;
            
            SimulationEntity entity = originalChunk.Entities[entityKey];
            
            //Remove from original chunk
            originalChunk.Entities.Remove(entityKey);
            
            //Add to new chunk
            newChunk.Entities.Add(entityKey, entity);
            
        }
        
        #endregion
        
        #region ChunkFromChunkLocation

        public Chunk ChunkFromChunkLocation(int chunkX, int chunkY)
        {
            if (chunkX < 0 || chunkY < 0)
                return null;

            if (chunkX > WorldOptions.WorldChunkWidth - 1 || chunkY > WorldOptions.WorldChunkHeight - 1)
                return null;

            return ChunkMap[chunkX, chunkY];
        }
        
        #endregion
        
        #region ChunkKeyFromChunkLocation

        public string ChunkKeyFromChunkLocation(int chunkX, int chunkY)
        {
            if (chunkX < 0 || chunkY < 0)
                return null;

            if (chunkX > WorldOptions.WorldChunkWidth - 1 || chunkY > WorldOptions.WorldChunkHeight - 1)
                return null;

            return ChunkMap[chunkX, chunkY].ChunkKey;
        }
        
        #endregion
        
        #region ChunkFromBlockLocation

        public Chunk ChunkFromTileLocation(int blockX, int blockY)
        {
            int chunkX = blockX / WorldOptions.ChunkSize;
            int chunkY = blockY / WorldOptions.ChunkSize;

            return ChunkFromChunkLocation(chunkX, chunkY);
        }
        
        #endregion
        
        #region ChunkFromPixelLocation

        public Chunk ChunkFromPixelLocation(int pixelX, int pixelY)
        {
            return ChunkFromTileLocation(pixelX/WorldOptions.WorldScale, pixelY/WorldOptions.WorldScale);
        }
        
        #endregion
        
        #region TranslatePosition

        /// <summary>
        /// Finds information about a blocks position based on a world block position
        /// </summary>
        /// <param name="xBlock">World block x position</param>
        /// <param name="yBlock">World block y position</param>
        /// <returns>A tuple representing the chunk x/y and the relative x/y blocks in the chunk</returns>
        public Tuple<int,int,int,int> TranslateBlockPosition(int xBlock, int yBlock)
        {
            return new Tuple<int, int, int, int>(
                xBlock/WorldOptions.ChunkSize,
                yBlock/WorldOptions.ChunkSize,
                xBlock % WorldOptions.ChunkSize,
                yBlock % WorldOptions.ChunkSize);
        }
        
        #endregion 
        
        #region TranslatePixelPosition

        /// <summary>
        /// Finds information about a block and chunk position based on a pixel world position
        /// </summary>
        /// <param name="xPixel">Pixel x world position</param>
        /// <param name="yPixel">Pixel y world postiion</param>
        /// <returns>A tuple representing the chunk x/y and the relative x/y blocks in the chunk</returns>
        public Tuple<int, int, int, int> TranslatePixelPosition(int xPixel, int yPixel)
        {
            return TranslateBlockPosition(xPixel / WorldOptions.WorldScale, yPixel / WorldOptions.WorldScale);
        }
        
        #endregion
        
        #region GetHardness
        
        public int GetHardness(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? ChunkMap[chunkX,chunkY].Hardness[relativeX, relativeY] : 0;
        }
        
        #endregion
        
        #region GetLight
        
        public int GetLight(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? ChunkMap[chunkX, chunkY].Light[relativeX, relativeY] : 0;
        }
        
        #endregion
        
        #region GetWall

        public int GetWall(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            return ChunkExist(chunkX, chunkY) ? ChunkMap[chunkX, chunkY].Wall[relativeX, relativeY] : 0;
        }
        
        #endregion

        #region GetBlock
        
        public WorldTile GetBlock(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            
            return ChunkExist(chunkX, chunkY) 
                ?  (WorldTile)ChunkMap[chunkX, chunkY].Block[relativeX, relativeY] 
                : WorldTile.Air;
        }

        #endregion

        #region SetHardness
        
        public void SetHardness(int xBlock, int yBlock, ushort hardnessValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))    
                ChunkMap[chunkX, chunkY].Hardness[relativeX, relativeY] = hardnessValue;
        }
        
        #endregion
        
        #region SetLight

        public void SetLight(int xBlock, int yBlock, byte lightValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                ChunkMap[chunkX, chunkY].Light[relativeX, relativeY] = lightValue;
        }
        
        #endregion
        
        #region SetWall

        public void SetWall(int xBlock, int yBlock, ushort wallValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                ChunkMap[chunkX, chunkY].Wall[relativeX, relativeY] = wallValue;
        }
        
        #endregion
        
        #region SetBlock

        public void SetBlock(int xBlock, int yBlock, WorldTile blockValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            
            if(ChunkExist(chunkX,chunkY))
                ChunkMap[chunkX, chunkY].Block[relativeX, relativeY] = (ushort)blockValue;
        }

        #endregion
        
        #region SetAll
        
        public void SetAll(int xBlock, int yBlock, ushort hardnessValue, byte lightValue, ushort wallValue, WorldTile tile)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslateBlockPosition(xBlock, yBlock);
            ChunkMap[chunkX, chunkY].Hardness[relativeX, relativeY] = hardnessValue;
            ChunkMap[chunkX, chunkY].Light[relativeX, relativeY] = lightValue;
            ChunkMap[chunkX, chunkY].Wall[relativeX, relativeY] = wallValue;
            ChunkMap[chunkX, chunkY].Block[relativeX, relativeY] = (ushort)tile;
        }

        #endregion

        #region ChunkExist

        private bool ChunkExist(int chunkX, int chunkY)
        {
            return ChunkMap[chunkX,chunkY] != null;
        }
        
        #endregion
    }

}
