using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Math;
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

        public void Draw(SpriteBatch graphics, WorldManager world, List<ClientEntity> entities, BFBContentManager content)
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

            #region Debug
            
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
                if (!Debug)
                    entity.Draw(graphics, _tileScale/GraphicsScale);
                else
                    entity.DebugDraw(graphics, content,_tileScale/GraphicsScale, _tileScale);
            }

            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
        }
        
        #endregion
        
    }
}