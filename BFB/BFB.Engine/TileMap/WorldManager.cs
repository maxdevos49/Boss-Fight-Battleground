using System;
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
        
        private Chunk[,] _chunks;
        public Action<string> WorldGeneratorCallback { get; set; }
        
        private readonly WorldGenerator _worldGenerator;

        #endregion
        
        #region Constructor
        
        public WorldManager(WorldOptions worldOptions)
        {
            WorldOptions = worldOptions;
            _worldGenerator = worldOptions.WorldGenerator?.Invoke(WorldOptions);
            _chunks = new Chunk[WorldOptions.WorldChunkWidth, WorldOptions.WorldChunkHeight];
        }
        
        #endregion

        //Move to world generator?
        #region GenerateWorld
        
        /**
         * Generates the entire map at once
         */
        public void GenerateWorld()
        {
            if (_worldGenerator == null)
                return;
            
            int previousPercent = 0;
            float progress = 0;
            for (int y = 0; y < WorldOptions.WorldChunkHeight; y++)
            {
                for (int x = 0; x < WorldOptions.WorldChunkWidth; x++)
                {
                    _chunks[x, y] = _worldGenerator.GenerateChunk(x * WorldOptions.ChunkSize, y * WorldOptions.ChunkSize);

                    int currentPercent = (int)(progress++ / (WorldOptions.WorldChunkHeight * WorldOptions.WorldChunkWidth) * 100f);
                    
                    if (previousPercent == currentPercent) continue;
                    
                    WorldGeneratorCallback?.Invoke(currentPercent + "%");
                    previousPercent = currentPercent;
                }
            }
            
            WorldGeneratorCallback?.Invoke("100% - Generation Complete");
        }
        
        #endregion
        
        #region GetChunks
        
        public Chunk[,] GetChunks()
        {
            return _chunks;
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
                Chunks = GetChunks()
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
                _chunks = worldData.Chunks;
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

//        #region MoveEntity
//
//        /**
//         * Moves entity from one chunk to another
//         */
//        public void MoveEntity(string entityKey, Chunk originalChunk, Chunk newChunk)
//        {
//            if (!originalChunk.Entities.ContainsKey(entityKey))
//                return;
//            
//            ServerEntity entity = originalChunk.Entities[entityKey];
//            
//            //Remove from original chunk
//            originalChunk.Entities.Remove(entityKey);
//            
//            //Add to new chunk
//            newChunk.Entities.Add(entityKey, entity);
//            
//        }
//        
//        #endregion
        
        #region ChunkFromBlockLocation

        public Chunk ChunkFromTileLocation(int blockX, int blockY)
        {
            int chunkX = blockX / WorldOptions.ChunkSize;
            int chunkY = blockY / WorldOptions.ChunkSize;

            if (chunkX < 0 || chunkX > WorldOptions.WorldChunkWidth)
                return null;

            if (chunkY < 0 || chunkY > WorldOptions.WorldChunkHeight)
                return null;
            
            return _chunks[chunkX, chunkY];
        }
        
        #endregion
        
        #region TranslatePosition

        private Tuple<int,int,int,int> TranslatePosition(int xBlock, int yBlock)
        {
            return new Tuple<int, int, int, int>(
                xBlock/WorldOptions.ChunkSize,
                yBlock/WorldOptions.ChunkSize,
                xBlock % WorldOptions.ChunkSize,
                yBlock % WorldOptions.ChunkSize);
        }
        
        #endregion 
        
        #region GetHardness
        
        public int GetHardness(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? _chunks[chunkX,chunkY].Hardness[relativeX, relativeY] : 0;
        }
        
        #endregion
        
        #region GetLight
        
        public int GetLight(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? _chunks[chunkX, chunkY].Light[relativeX, relativeY] : 0;
        }
        
        #endregion
        
        #region GetWall

        public int GetWall(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX, chunkY) ? _chunks[chunkX, chunkY].Wall[relativeX, relativeY] : 0;
        }
        
        #endregion

        #region GetBlock
        
        public WorldTile GetBlock(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            
            return ChunkExist(chunkX, chunkY) 
                ?  (WorldTile)_chunks[chunkX, chunkY].Block[relativeX, relativeY] 
                : WorldTile.Air;
        }

        #endregion

        #region SetHardness
        
        public void SetHardness(int xBlock, int yBlock, ushort hardnessValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))    
                _chunks[chunkX, chunkY].Hardness[relativeX, relativeY] = hardnessValue;
        }
        
        #endregion
        
        #region SetLight

        public void SetLight(int xBlock, int yBlock, byte lightValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                _chunks[chunkX, chunkY].Light[relativeX, relativeY] = lightValue;
        }
        
        #endregion
        
        #region SetWall

        public void SetWall(int xBlock, int yBlock, int wallValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                _chunks[chunkX, chunkY].Wall[relativeX, relativeY] = wallValue;
        }
        
        #endregion
        
        #region SetBlock

        public void SetBlock(int xBlock, int yBlock, WorldTile blockValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            
            if(ChunkExist(chunkX,chunkY))
                _chunks[chunkX, chunkY].Block[relativeX, relativeY] = (int)blockValue;
        }

        #endregion
        
        #region SetAll
        
        public void SetAll(int xBlock, int yBlock, ushort hardnessValue, byte lightValue, int wallValue, WorldTile tile)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            _chunks[chunkX, chunkY].Hardness[relativeX, relativeY] = hardnessValue;
            _chunks[chunkX, chunkY].Light[relativeX, relativeY] = lightValue;
            _chunks[chunkX, chunkY].Wall[relativeX, relativeY] = wallValue;
            _chunks[chunkX, chunkY].Block[relativeX, relativeY] = (int)tile;
        }

        #endregion

        #region ChunkExist

        private bool ChunkExist(int chunkX, int chunkY)
        {
            return _chunks[chunkX,chunkY] != null;
        }
        
        #endregion
    }

}
