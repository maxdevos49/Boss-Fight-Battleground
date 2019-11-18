using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.TileMap.Generators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.TileMap
{
    /// <summary>
    /// Handles Rendering the World efficiently with the camera
    /// </summary>
    public class WorldRenderer
    {
        
        public const float GraphicsScale = 15f;
        
        public bool Debug { get; set; }
        public readonly Camera Camera;

        private readonly int _tileScale;
        private readonly int _blockWidth;
        private readonly int _blockHeight;
 
        public WorldRenderer(WorldManager world, GraphicsDevice graphicsDevice)
        {
//            Debug = true;
            _tileScale = world.WorldOptions.WorldScale;
            _blockWidth = world.MapBlockWidth();
            _blockHeight = world.MapBlockHeight();
            
            Camera = new Camera(graphicsDevice, world.MapPixelWidth(), world.MapPixelHeight());
        }
        
        #region ViewPointToMapPoint

        public BfbVector ViewPointToMapPoint(BfbVector point)
        {
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
            //Interpolation
            foreach (ClientEntity entity in entities)
                entity.Update();
            
            //update camera
            Camera.Update(gameTime);
        }
        
        #endregion
        

        
        #region Draw

        public void Draw(SpriteBatch graphics, WorldManager world, IEnumerable<ClientEntity> entities, BFBContentManager content)
        {
            //Start different graphics layer
            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Camera.Transform);

            int xStart = (int)(Camera.Position.X - Camera.Origin.X)/_tileScale - 1;
            int xEnd = (int)(Camera .Position.X - Camera.Origin.X + Camera.ViewWidth)/_tileScale + 2;
            int yStart = (int)(Camera.Position.Y - Camera.Origin.Y)/_tileScale - 1;
            int yEnd = (int)(Camera .Position.Y - Camera.Origin.Y + Camera.ViewHeight)/_tileScale + 2;

            if (xStart < 0)
                xStart = 0;
            if (xEnd > _blockWidth)
                xEnd = _blockWidth;
            
            if (yStart < 0)
                yStart = 0;
            if (yEnd > _blockHeight)
                yEnd = _blockHeight;

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    int xPosition = x * _tileScale;
                    int yPosition = y * _tileScale;
                    
                    switch((WorldTile)world.GetWall(x, y))
                    {
                        case WorldTile.Dirt:
                            graphics.Draw(content.GetTexture("dirt"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                            graphics.Draw(content.GetTexture("default"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),new Color(0,0,0,0.4f));
                            break;
                        case WorldTile.Stone:
                            graphics.Draw(content.GetTexture("stone"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                            graphics.Draw(content.GetTexture("default"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),new Color(0,0,0,0.4f));
                            break;
                    }
                    

                    
                    switch(world.GetBlock(x, y))//TODO change so not switch statement but add a special content type for blocks
                    {
                        case WorldTile.Grass:
                            graphics.Draw(content.GetTexture("grass"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                            break;
                        case WorldTile.Dirt:
                            graphics.Draw(content.GetTexture("dirt"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                            break;
                        case WorldTile.Stone:
                            graphics.Draw(content.GetTexture("stone"), new Rectangle(xPosition,yPosition,_tileScale,_tileScale ),Color.White);
                            break;
                    }
               
                }
            }
            
            #region DebugTileMap
            
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
                            content.GetFont("default"), 
                            (int) world.GetBlock(x, y) + "",
                            new Vector2(xPosition, yPosition), 
                            Color.Black, 
                            0f, 
                            Vector2.Zero, 
                            0.5f,
                            SpriteEffects.None,
                            1);
                        
                        if(x % world.WorldOptions.ChunkSize == 0 && y % world.WorldOptions.ChunkSize == 0)
                            graphics.DrawBorder(
                                new Rectangle(
                                    xPosition, 
                                    yPosition,
                                    world.WorldOptions.ChunkSize * _tileScale,
                                    world.WorldOptions.ChunkSize * _tileScale), 
                                1, 
                                Color.Red,
                                content.GetTexture("default"));
                    }
                }
            }

            #endregion

            
            foreach (ClientEntity entity in entities)
            {
                if(!Debug)
                    entity.Draw(graphics, _tileScale/GraphicsScale);
                else
                    entity.DebugDraw(graphics, content,_tileScale/GraphicsScale, _tileScale);
            }

            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }
        
        #endregion
        
        #region DebugPanel

        public void DebugPanel(SpriteBatch graphics, GameTime time ,WorldManager world, ClientEntity entity, List<ClientEntity> entities, ClientSocketManager connection, PlayerState playerState, BFBContentManager content)
        {
            int xPos = 5;
            int yPos = 120;
            int offset = 24;
            
            graphics.DrawBackedText($"Client FPS: {System.Math.Round(1f/(float)time.ElapsedGameTime.TotalSeconds)}/60", new BfbVector(xPos,yPos),content,0.5f);
            graphics.DrawBackedText("Server TPS: {}/20", new BfbVector(xPos,yPos += offset),content,0.5f);
            graphics.DrawBackedText($"X: {Camera.Position.X}, Y: {Camera.Position.Y}", new BfbVector(xPos,yPos += offset),content,0.5f);
            Chunk chunk = world.ChunkFromPixelLocation((int) Camera.Position.X, (int) Camera.Position.Y);
            graphics.DrawBackedText($"Chunk-X: {chunk?.ChunkX ?? 0}, Chunk-Y: {chunk?.ChunkY ?? 0}", new BfbVector(xPos,yPos +=offset),content,0.5f);
            graphics.DrawBackedText($"Velocity-X: {entity.Velocity.X}, Velocity-Y: {entity.Velocity.Y}", new BfbVector(xPos,yPos += offset),content,0.5f);
            graphics.DrawBackedText($"Facing: {entity.Facing}", new BfbVector(xPos,yPos += offset),content,0.5f);
            Tuple<int, int> location = world.BlockLocationFromPixel((int)playerState.Mouse.X, (int)playerState.Mouse.Y);
            graphics.DrawBackedText($"Mouse-X: {(int)playerState.Mouse.X}, Mouse-Y: {(int)playerState.Mouse.Y}", new BfbVector(xPos,yPos += offset),content,0.5f);
            graphics.DrawBackedText($"Block-X: {location.Item1}, Block-Y: {location.Item2}, Block: {world.GetBlock(location.Item1,location.Item2)}, Wall: {(WorldTile)world.GetWall(location.Item1,location.Item2)}", new BfbVector(xPos,yPos += offset),content,0.5f );
            graphics.DrawBackedText($"Entities: {entities.Count}, Players: {entities.Count(x => x.EntityType == EntityType.Player)}, Items: {entities.Count(x => x.EntityType == EntityType.Item)}", new BfbVector(xPos,yPos += offset),content,0.5f);
            
            graphics.DrawBackedText("Press F3 to exit Debug", new BfbVector(xPos,yPos += offset*2),content,0.5f);
            
        }
        
        #endregion
        
    }
}