using System;
using System.IO;
using BFB.Engine.Content;
using BFB.Engine.TileMap.Generators;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace BFB.Engine.TileMap
{

    public class WorldManager
    {
        public WorldOptions WorldOptions { get; private set; }
        private Chunk[,] _world;
        public Action<string> WorldGeneratorCallback { get; set; }
        
        private readonly WorldGenerator _worldGenerator;

        public WorldManager(WorldOptions worldOptions)
        {
            WorldOptions = worldOptions;
            _worldGenerator = worldOptions.WorldGenerator?.Invoke(WorldOptions);
            _world = new Chunk[WorldOptions.WorldChunkWidth, WorldOptions.WorldChunkHeight];
        }

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
                    _world[x, y] = _worldGenerator.GenerateChunk(x * WorldOptions.ChunkSize, y * WorldOptions.ChunkSize);

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
            return _world;
        }
        
        #endregion
        
        #region SaveWorld
        
        /**
         * Saves the current state of the world with a given in the world folder
         */
        public bool SaveWorld(string fileName)
        {
            WorldData worldData = new WorldData
            {
                WorldConfig = WorldOptions,
                Chunks = _world
            };

            string serializedWorldData = JsonConvert.SerializeObject(worldData);

            try
            {
                Console.WriteLine(Directory.GetCurrentDirectory() + "/Worlds/" + fileName);
                File.WriteAllText(Directory.GetCurrentDirectory() + "/Worlds/" + fileName, serializedWorldData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        
        #endregion

        #region LoadWorld
        
        public void LoadWorld(string filePath)
        {
            //TODO check if file exist
            
            string json;
            
            //Get file for Parsing
            using (StreamReader r = new StreamReader("~/Worlds/" + filePath))
            {
                json = r.ReadToEnd();
            }
            
            WorldData worldData = JsonConvert.DeserializeObject<WorldData>(json);

            WorldOptions = worldData.WorldConfig;
            _world = worldData.Chunks;

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
            return ChunkExist(chunkX,chunkY) ? _world[chunkX,chunkY].GetTileHardness(relativeX, relativeY) : 0;
        }
        
        #endregion
        
        #region GetLight
        
        public int GetLight(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? _world[chunkX, chunkY].GetTileLight(relativeX, relativeY) : 0;
        }
        
        #endregion
        
        #region GetWall

        public int GetWall(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX, chunkY) ? _world[chunkX, chunkY].GetTileWall(relativeX, relativeY) : 0;
        }
        
        #endregion

        #region GetBlock
        
        public WorldTile GetBlock(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            
            return ChunkExist(chunkX, chunkY) 
                ?  _world[chunkX, chunkY].GetTileBlock(relativeX, relativeY) 
                : WorldTile.Air;
        }

        #endregion

        #region SetHardness
        
        public void SetHardness(int xBlock, int yBlock, int hardnessValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))    
                _world[chunkX, chunkY].SetTileHardness(relativeX, relativeY, hardnessValue);
        }
        
        #endregion
        
        #region SetLight

        public void SetLight(int xBlock, int yBlock, int lightValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                _world[chunkX, chunkY].SetTileLight(relativeX, relativeY, lightValue);
        }
        
        #endregion
        
        #region SetWall

        public void SetWall(int xBlock, int yBlock, int wallValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                _world[chunkX, chunkY].SetTileWall(relativeX, relativeY, wallValue);
        }
        
        #endregion
        
        #region SetBlock

        public void SetBlock(int xBlock, int yBlock, WorldTile blockValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            
            if(ChunkExist(chunkX,chunkY))
                _world[chunkX, chunkY].SetTileBlock(relativeX, relativeY, blockValue);
        }

        #endregion
        
        #region SetAll
        
        public void SetAll(int xBlock, int yBlock, int hardnessValue, int lightValue, int wallValue, WorldTile tile)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            _world[chunkX, chunkY].SetTileHardness(relativeX, relativeY, hardnessValue);
            _world[chunkX, chunkY].SetTileLight(relativeX, relativeY, lightValue);
            _world[chunkX, chunkY].SetTileWall(relativeX, relativeY, wallValue);
            _world[chunkX, chunkY].SetTileBlock(relativeX, relativeY, tile);
        }

        #endregion

        #region ChunkExist

        private bool ChunkExist(int chunkX, int chunkY)
        {
            return _world[chunkX,chunkY] != null;
        }
        
        #endregion
        
    }
    
}
