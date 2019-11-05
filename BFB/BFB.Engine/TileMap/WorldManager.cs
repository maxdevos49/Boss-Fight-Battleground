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
        
        /// <summary>
        /// The options for the world.
        /// </summary>
        public WorldOptions WorldOptions { get; private set; }
        
        /// <summary>
        /// The map for a chunk.
        /// </summary>
        public Chunk[,] ChunkMap { get; private set; }
        /// <summary>
        /// The index for the chunk.
        /// </summary>
        public Dictionary<string, Chunk> ChunkIndex { get; }
        /// <summary>
        /// The callback action for the world generator.
        /// </summary>
        public Action<string> WorldGeneratorCallback { get; set; }
        
        
        private readonly WorldGenerator _worldGenerator;

        #endregion
        
        #region Constructor
        
        /// <summary>
        /// The constructor for worldManager.
        /// </summary>
        /// <param name="worldOptions">The options for the world configuration.</param>
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
        

        /// <summary>
        /// Generates the entire map at once.
        /// </summary>
        /// <param name="progressCallback">Gets the progress from the world generation.</param>
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


        /// <summary>
        /// Adds each chunk to a dictionary so depending on if you have the x/y position or the chunk key then you always have O(1) lookup.
        /// </summary>
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
        

        /// <summary>
        /// Saves the current state of the world with a given name in the world folder.
        /// </summary>
        /// <param name="fileName">The name to save the file as.</param>
        /// <returns>Returns true if world is saved, false otherwise.</returns>
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
        
        /// <summary>
        /// Loads a world from a given filename.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>Returns true if file is loaded.</returns>
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
        
        /// <summary>
        /// Moves entity from one chunk to another.
        /// </summary>
        /// <param name="entityKey">The given entity.</param>
        /// <param name="originalChunk">The chunk the entity is in.</param>
        /// <param name="newChunk">The chunk the entity is going to be in.</param>
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

        /// <summary>
        /// Gets a chunk from a chunk location.
        /// </summary>
        /// <param name="chunkX">The X location of the chunk.</param>
        /// <param name="chunkY">The Y location of the chunk.</param>
        /// <returns>Returns the specified chunk.</returns>
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

        /// <summary>
        /// Retrieves the key of the chunk location.
        /// </summary>
        /// <param name="chunkX">The X location of the chunk.</param>
        /// <param name="chunkY">The Y location of the chunk.</param>
        /// <returns>Returns the key of the specified chunk.</returns>
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

        /// <summary>
        /// Retrieves the chunk where the tile is located.
        /// </summary>
        /// <param name="blockX">The X block location of the tileMap.</param>
        /// <param name="blockY">The Y block location of the tileMap.</param>
        /// <returns>Returns the chunk from the specified block locations.</returns>
        public Chunk ChunkFromTileLocation(int blockX, int blockY)
        {
            int chunkX = blockX / WorldOptions.ChunkSize;
            int chunkY = blockY / WorldOptions.ChunkSize;

            return ChunkFromChunkLocation(chunkX, chunkY);
        }
        
        #endregion
        
        #region ChunkFromPixelLocation

        /// <summary>
        /// Retrieves the chunk from the pixel location on the screen.
        /// </summary>
        /// <param name="pixelX">The X pixel location on the screen.</param>
        /// <param name="pixelY">The Y pixel location on the screen.</param>
        /// <returns>Returns the chunk from the pixel locations.</returns>
        public Chunk ChunkFromPixelLocation(int pixelX, int pixelY)
        {
            return ChunkFromTileLocation(pixelX/WorldOptions.WorldScale, pixelY/WorldOptions.WorldScale);
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
        /// <summary>
        /// Retrieves the hardness value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <returns>Returns the hardness value based on the block locations.</returns>
        public int GetHardness(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? ChunkMap[chunkX,chunkY].Hardness[relativeX, relativeY] : 0;
        }
        
        #endregion
        
        #region GetLight
        /// <summary>
        /// Retrieves the light value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <returns>Returns the light value based on the block locations.</returns>
        public int GetLight(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX,chunkY) ? ChunkMap[chunkX, chunkY].Light[relativeX, relativeY] : 0;
        }
        
        #endregion
        
        #region GetWall
        /// <summary>
        /// Retrieves the wall value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <returns>Returns the wall value based on the block locations.</returns>
        public int GetWall(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            return ChunkExist(chunkX, chunkY) ? ChunkMap[chunkX, chunkY].Wall[relativeX, relativeY] : 0;
        }
        
        #endregion

        #region GetBlock
        /// <summary>
        /// Retrieves the block value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <returns>Returns the block value based on the block locations.</returns>
        public WorldTile GetBlock(int xBlock, int yBlock)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            
            return ChunkExist(chunkX, chunkY) 
                ?  (WorldTile)ChunkMap[chunkX, chunkY].Block[relativeX, relativeY] 
                : WorldTile.Air;
        }

        #endregion

        #region SetHardness
        /// <summary>
        /// Sets the hardness value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <param name="hardnessValue">The value of the hardness to be set.</param>
        public void SetHardness(int xBlock, int yBlock, ushort hardnessValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))    
                ChunkMap[chunkX, chunkY].Hardness[relativeX, relativeY] = hardnessValue;
        }
        
        #endregion
        
        #region SetLight
        /// <summary>
        /// Sets the light value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <param name="lightValue">The value of the light to be set.</param>
        public void SetLight(int xBlock, int yBlock, byte lightValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                ChunkMap[chunkX, chunkY].Light[relativeX, relativeY] = lightValue;
        }
        
        #endregion
        
        #region SetWall
        /// <summary>
        /// Sets the wall value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <param name="wallValue">The value of the wall to be set.</param>
        public void SetWall(int xBlock, int yBlock, ushort wallValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            if(ChunkExist(chunkX,chunkY))
                ChunkMap[chunkX, chunkY].Wall[relativeX, relativeY] = wallValue;
        }
        
        #endregion
        
        #region SetBlock
        /// <summary>
        /// Sets the block value of a block.
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <param name="blockValue">The value of the block to be set.</param>
        public void SetBlock(int xBlock, int yBlock, WorldTile blockValue)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            
            if(ChunkExist(chunkX,chunkY))
                ChunkMap[chunkX, chunkY].Block[relativeX, relativeY] = (ushort)blockValue;
        }

        #endregion
        
        #region SetAll
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xBlock">The X location of a block.</param>
        /// <param name="yBlock">The Y location of a block.</param>
        /// <param name="hardnessValue">The value of the hardness to be set.</param>
        /// <param name="lightValue">The value of the light to be set.</param>
        /// <param name="wallValue">The value of the wall to be set.</param>
        /// <param name="tile">The value of the block to be set.</param>
        public void SetAll(int xBlock, int yBlock, ushort hardnessValue, byte lightValue, ushort wallValue, WorldTile tile)
        {
            (int chunkX, int chunkY, int relativeX, int relativeY) = TranslatePosition(xBlock, yBlock);
            ChunkMap[chunkX, chunkY].Hardness[relativeX, relativeY] = hardnessValue;
            ChunkMap[chunkX, chunkY].Light[relativeX, relativeY] = lightValue;
            ChunkMap[chunkX, chunkY].Wall[relativeX, relativeY] = wallValue;
            ChunkMap[chunkX, chunkY].Block[relativeX, relativeY] = (ushort)tile;
        }

        #endregion

        #region ChunkExist
        /// <summary>
        /// Checks if a chunk exists.
        /// </summary>
        /// <param name="chunkX">The X location of a chunk.</param>
        /// <param name="chunkY">The Y location of a chunk.</param>
        /// <returns>Returns true if the chunk does exist.</returns>
        private bool ChunkExist(int chunkX, int chunkY)
        {
            return ChunkMap[chunkX,chunkY] != null;
        }
        
        #endregion
    }

}
