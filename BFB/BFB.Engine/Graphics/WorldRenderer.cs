using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Inventory;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.TileMap;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Graphics
{
    /// <summary>
    /// Handles Rendering the World efficiently with the camera
    /// </summary>
    public class WorldRenderer
    {
        #region Properties
        
        [UsedImplicitly]
        public const float GraphicsScale = 15f;
        public bool Debug { get; set; }
        public Camera Camera { get; private set; }

        private BFBContentManager _content;

        private bool _init;
        private int _tileScale;
        private int _blockWidth;
        private int _blockHeight;

        #endregion
 
        #region Constructor
        
        public WorldRenderer()
        {
            _init = false;
        }
        
        #endregion
        
        #region Init

        public void Init(WorldManager world, BFBContentManager content,GraphicsDevice graphicsDevice)
        {
            _content = content;
            Camera = new Camera(graphicsDevice, world.MapPixelWidth(), world.MapPixelHeight());
            
            _tileScale = world.WorldOptions.WorldScale;
            _blockWidth = world.MapBlockWidth();
            _blockHeight = world.MapBlockHeight();
            
            _init = true;
        }
        
        #endregion
        
        #region ViewPointToMapPoint

        public BfbVector ViewPointToMapPoint(BfbVector point)
        {
            if (!_init)
                return null;
            
            BfbVector translatedPoint = new BfbVector(point.ToVector2());

            translatedPoint.X /= Camera.GetScale().X;
            translatedPoint.Y /= Camera.GetScale().Y;

            translatedPoint.X += Camera.Position.X - Camera.Origin.X;
            translatedPoint.Y += Camera.Position.Y - Camera.Origin.Y;

            return translatedPoint;
        }
        
        #endregion
        
        #region Update

        public void Update(GameTime gameTime, List<ClientEntity> entities)
        {
            if (!_init)
                return;
            
            //Interpolation
            foreach (ClientEntity entity in entities)
                entity.Update();
            
            //update camera
            Camera.Update(gameTime);
        }
        
        #endregion

        
        #region Draw

        public void Draw(SpriteBatch graphics, GameTime time, WorldManager world, List<ClientEntity> entities, ClientEntity playerEntity, ControlState input, ClientSocketManager clientSocket = null)
        {
            if (!_init)
                return;
            
            float worldScale = _tileScale / GraphicsScale;
            
            #region graphics.Begin()
            //Start different graphics layer
            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Camera.Transform);
            #endregion
            
            #region DrawRanges
            
            int xStart = Camera.Left / _tileScale - 1;
            int xEnd = Camera.Right / _tileScale + 2;
            
            int yStart = Camera.Top/_tileScale - 1;
            int yEnd = Camera.Bottom/_tileScale + 2;

            if (xStart < 0)
                xStart = 0;
            
            if (xEnd > _blockWidth)
                xEnd = _blockWidth;
            
            if (yStart < 0)
                yStart = 0;
            if (yEnd > _blockHeight)
                yEnd = _blockHeight;
            
            #endregion
            
            #region Render Walls + Blocks

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    int xPosition = x * _tileScale;
                    int yPosition = y * _tileScale;

                    if (world.GetBlock(x, y) == WorldTile.Air)
                    {

                        #region Render Walls
                        
                        switch ((WorldTile) world.GetWall(x, y))
                        {
                            case WorldTile.Dirt:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Dirt"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale), Color.White);
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale),
                                    new Color(0, 0, 0, 0.4f));
                                break;
                            case WorldTile.Stone:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Stone"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale), Color.White);
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale),
                                    new Color(0, 0, 0, 0.4f));
                                break;
                            case WorldTile.Wood:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Wood"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale), Color.White);
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale),
                                    new Color(0, 0, 0, 0.4f));
                                break;
                            case WorldTile.Leaves:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Leaves"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale), Color.White);
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale),
                                    new Color(0, 0, 0, 0.4f));
                                break;
                            case WorldTile.Plank:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Plank"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale), Color.White);
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(xPosition, yPosition, _tileScale, _tileScale),
                                    new Color(0, 0, 0, 0.4f));
                                break;
                        }
                        
                        #endregion
                    }
                    else
                    {
                        #region Render Blocks
                        
                        switch(world.GetBlock(x, y))//TODO change so not switch statement but add a special content type for blocks
                        {
                            case WorldTile.Grass:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Grass"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                                break;
                            case WorldTile.Dirt:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Dirt"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                                break;
                            case WorldTile.Stone:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Stone"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                                break;
                            case WorldTile.Wood:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Wood"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                                break;
                            case WorldTile.Leaves:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Leaves"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                                break;
                            case WorldTile.Plank:
                                graphics.DrawAtlas(_content.GetAtlasTexture("Tiles:Plank"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                                break;
                        }
                    #endregion
                    }
                }
            }
            
            #endregion
            
            #region Mouse ToolTip


            if (playerEntity.Meta != null && playerEntity.Meta.Holding.ItemType != ItemType.Unknown)
            {
                
                //Get proper mouse coordinates
                BfbVector mouse = ViewPointToMapPoint(input.Mouse);
                Tuple<int, int> block = world.BlockLocationFromPixel((int) mouse.X, (int) mouse.Y);
                
//                Console.WriteLine("Mouse: " + mouse.X + " - "+ mouse.Y);
//                Console.WriteLine("Block: "+ block?.Item1 + " - "+ block?.Item2);
                
                //if the mouse is inside the map
                if (block != null)
                {
                    int playerX = (int)(playerEntity.Position.X + playerEntity.Width / 2f);
                    int playerY = (int)(playerEntity.Position.Y + playerEntity.Height / 2f);
                    int blockPixelX = block.Item1 * _tileScale;//x position of block mouse is over
                    int blockPixelY = block.Item2 * _tileScale;//y position of block mouse is over
                    
                    int distance = (int) System.Math.Sqrt(System.Math.Pow(playerX - blockPixelX, 2) + System.Math.Pow(playerY - blockPixelY, 2)) / _tileScale;
                    int reach = playerEntity.Meta.Holding.Reach;

                    switch (playerEntity.Meta.Holding.ItemType)
                    {
                        case ItemType.Block:
                            
                            if (world.GetBlock(block.Item1, block.Item2) == WorldTile.Air)
                            {
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(blockPixelX, blockPixelY, _tileScale, _tileScale),
                                    distance > reach ? new Color(255, 0, 0, 0.2f) :
                                        new Color(0, 0, 0, 0.2f));
                            }
                            else
                            {
                                float progress = playerEntity.Meta.Holding.Progress;
                                

                                if (progress < 0.05f)
                                {
                                    graphics.DrawBorder(
                                        new Rectangle(blockPixelX, blockPixelY, _tileScale, _tileScale),
                                        2,
                                        new Color(0,0,0,0.6f), 
                                        _content.GetTexture("default"));
                                }
                                else
                                {
                                    graphics.Draw(_content.GetTexture("default"),
                                        new Rectangle(blockPixelX + (int)(_tileScale*(1-progress))/2, blockPixelY + (int)(_tileScale*(1-progress))/2, (int)(_tileScale*progress), (int)(_tileScale*progress)),
                                        new Color(0,0,0,0.4f));
                                }
                            }
                            break;
                        case ItemType.Wall:

                            if (world.GetBlock(block.Item1, block.Item2) == WorldTile.Air && (WorldTile)world.GetWall(block.Item1, block.Item2) == WorldTile.Air)
                            {
                                graphics.Draw(_content.GetTexture("default"),
                                    new Rectangle(blockPixelX, blockPixelY, _tileScale, _tileScale),
                                    distance > reach ? new Color(255, 0, 0, 0.2f) :
                                        new Color(0, 0, 0, 0.2f));
                            } else
                            {
                                float progress = playerEntity.Meta.Holding.Progress;
                                
                                if (progress < 0.05f)
                                {
                                    graphics.DrawBorder(
                                        new Rectangle(blockPixelX, blockPixelY, _tileScale, _tileScale),
                                        2,
                                        new Color(0,0,0,0.6f), 
                                        _content.GetTexture("default"));
                                }
                                else
                                {
                                    graphics.Draw(_content.GetTexture("default"),
                                        new Rectangle(blockPixelX + (int)(_tileScale*(1-progress))/2, blockPixelY + (int)(_tileScale*(1-progress))/2, (int)(_tileScale*progress), (int)(_tileScale*progress)),
                                        new Color(0,0,0,0.4f));
                                }
                            }

                            break;
                        case ItemType.Tool:
                            Console.WriteLine("Tool");

                            graphics.DrawAtlas(_content.GetAtlasTexture(playerEntity.Meta.Holding.AtlasKey),
                                new Rectangle((int)mouse.X,(int) mouse.Y, _tileScale, _tileScale),
                                distance > reach ? 
                                    new Color(255, 0, 0, 0.1f)
                                    : new Color(255, 255, 255, 0.1f));
                            
                            break;
                    }
                    
                }
            }

            #endregion
            
            #region Map Debug
            
            if(Debug)
                MapDebug(graphics,world,xStart,xEnd,yStart,yEnd);

            #endregion

            #region Render Entities
            
            foreach (ClientEntity entity in entities.OrderBy(x => x.Position.Y).ThenBy(x => x.EntityType))
            {
                if(!Debug)
                    entity.Draw(graphics, _content, worldScale);
                else
                    entity.DebugDraw(graphics, _content, worldScale, _tileScale);
            }
            
            #endregion
            
            #region graphics.End()
            
            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
            #endregion
            
            #region Debug Panel
            
            if(Debug)
                DebugPanel(graphics,time,world,playerEntity,entities,clientSocket,input);
            
            #endregion
        }
        
        #endregion
        
        #region MapDebug

        private void MapDebug(SpriteBatch graphics,WorldManager world, int xStart, int xEnd, int yStart, int yEnd)
        {
            if (Debug)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    for (int x = xStart; x < xEnd; x++)
                    {
                        int xPosition = x * _tileScale;
                        int yPosition = y * _tileScale;

                        //Block Values
                        graphics.DrawString(
                            _content.GetFont("default"),
                            (int) world.GetBlock(x, y) + "",
                            new Vector2(xPosition, yPosition),
                            Color.Black,
                            0f,
                            Vector2.Zero,
                            0.5f,
                            SpriteEffects.None,
                            1);

                        if (x % world.WorldOptions.ChunkSize == 0 && y % world.WorldOptions.ChunkSize == 0)
                            graphics.DrawBorder(
                                new Rectangle(
                                    xPosition,
                                    yPosition,
                                    world.WorldOptions.ChunkSize * _tileScale,
                                    world.WorldOptions.ChunkSize * _tileScale),
                                1,
                                Color.Red,
                                _content.GetTexture("default"));
                    }
                }
            }
        }
        
        #endregion
        
        #region DebugPanel

        private void DebugPanel(SpriteBatch graphics, GameTime time ,WorldManager world, ClientEntity entity, List<ClientEntity> entities, ClientSocketManager connection, ControlState input)
        {
            const int xPos = 5;
            const int offset = 24;
            int yPos = 120;

            graphics.DrawBackedText($"Client FPS: {System.Math.Round(1f/(float)time.ElapsedGameTime.TotalSeconds)}/60", new BfbVector(xPos,yPos),_content,0.5f);
            graphics.DrawBackedText("Server TPS: {}/20", new BfbVector(xPos,yPos += offset),_content,0.5f);
            graphics.DrawBackedText($"X: {entity.Left}, Y: {entity.Top}", new BfbVector(xPos,yPos += offset),_content,0.5f);
            Chunk chunk = world.ChunkFromPixelLocation((int) entity.Left, (int)entity.Top);
            graphics.DrawBackedText($"Chunk-X: {chunk?.ChunkX ?? 0}, Chunk-Y: {chunk?.ChunkY ?? 0}", new BfbVector(xPos,yPos +=offset),_content,0.5f);
            graphics.DrawBackedText($"Velocity-X: {entity.Velocity.X}, Velocity-Y: {entity.Velocity.Y}", new BfbVector(xPos,yPos += offset),_content,0.5f);
            graphics.DrawBackedText($"Facing: {entity.Facing}", new BfbVector(xPos,yPos += offset),_content,0.5f);
            BfbVector mouse = ViewPointToMapPoint(input.Mouse);
            (int blockX, int blockY) = world.BlockLocationFromPixel((int)mouse.X, (int)mouse.Y);
            graphics.DrawBackedText($"Mouse-X: {(int)mouse.X}, Mouse-Y: {(int)mouse.Y}", new BfbVector(xPos,yPos += offset),_content,0.5f);
            graphics.DrawBackedText($"Block-X: {blockX}, Block-Y: {blockY}, Block: {world.GetBlock(blockX,blockY)}, Wall: {(WorldTile)world.GetWall(blockX,blockY)}", new BfbVector(xPos,yPos += offset),_content,0.5f );
            graphics.DrawBackedText($"Entities: {entities.Count}, Players: {entities.Count(x => x.EntityType == EntityType.Player)}, Items: {entities.Count(x => x.EntityType == EntityType.Item)}, Mobs: {entities.Count(x => x.EntityType == EntityType.Mob)}", new BfbVector(xPos,yPos += offset),_content,0.5f);
            
            graphics.DrawBackedText("Press F3 to exit Debug", new BfbVector(xPos,yPos + offset*2),_content,0.5f);
            
        }
        
        #endregion
        
    }
}