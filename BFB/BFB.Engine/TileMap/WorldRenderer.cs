using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using BFB.Engine.Entity;
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

        public readonly Camera Camera;

        private readonly int _worldScale;
        private readonly int _blockWidth;
        private readonly int _blockHeight;
 
        public WorldRenderer(WorldManager world, GraphicsDevice graphicsDevice)
        {
            _worldScale = world.WorldOptions.WorldScale;
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
            
            //translatedPoint.Y -= (float)_worldScale / 2;//Not always works

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

            int xStart = (int)(Camera.Position.X - Camera.Origin.X)/_worldScale - 1;
            int xEnd = (int)(Camera .Position.X - Camera.Origin.X + Camera.ViewWidth)/_worldScale;
            int yStart = (int)(Camera.Position.Y - Camera.Origin.Y)/_worldScale - 1;
            int yEnd = (int)(Camera .Position.Y - Camera.Origin.Y + Camera.ViewHeight)/_worldScale;

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
                    int xPosition = x * _worldScale;
                    int yPosition = y * _worldScale;
                    
                    switch(world.GetBlock(x, y))//TODO change so not switch statement but add a special content for blocks
                    {
                        case WorldTile.Grass:
                            graphics.Draw(content.GetTexture("grass"), new Vector2(xPosition, yPosition), Color.White);
                            break;
                        case WorldTile.Dirt:
                            graphics.Draw(content.GetTexture("dirt"), new Vector2(xPosition, yPosition), Color.White);
                            break;
                        case WorldTile.Stone:
                            graphics.Draw(content.GetTexture("stone"), new Vector2(xPosition, yPosition), Color.White);
                            break;
                    }
                    
                }
            }
            
            foreach (ClientEntity entity in entities)
                entity.Draw(graphics);

            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
        }
        
        #endregion
        
    }
}