using BFB.Engine.Content;
using BFB.Engine.Helpers;
using BFB.Engine.Math;
using BFB.Engine.Simulation.GraphicsComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Entity
{
    /// <summary>
    /// Entities loaded into the client that are displayed to the user
    /// </summary>
    public class ClientEntity : Entity
    {
        #region Properties
        
        private readonly IGraphicsComponent _graphics;
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Creates a new ClientEntity
        /// </summary>
        /// <param name="entityId">Unique ID of this entity</param>
        /// <param name="options">Options Object of this entity</param>
        /// <param name="graphics">Graphics component for drawing</param>
        public ClientEntity(string entityId, EntityOptions options, IGraphicsComponent graphics) : base(entityId, options)
        {
            _graphics = graphics;
        }
        
        #endregion
        
        #region Update

        /// <summary>
        /// Extends Entity's Update() method
        /// Updates this ClientEntity's information
        /// </summary>
        public void Update()
        {
            _graphics?.Update(this);

            //Interpolation: server is 20 tps / client is 60 tps = 1/3
            float tickRateRatio = (float)1 / 3;
            Position.X += Velocity.X * tickRateRatio;
            Position.Y += Velocity.Y * tickRateRatio;
        }
        
        #endregion

        #region Draw

        public void Draw(SpriteBatch graphics, float worldScale)
        {
            _graphics?.Draw(this, graphics, worldScale);
        }
        
        #endregion

        public void DebugDraw(SpriteBatch graphics, BFBContentManager content, float worldScale, float tileSize)
        {

            int topBlockY = (int) System.Math.Floor(Top / tileSize);
            int leftBlockX = (int) System.Math.Floor(Left / tileSize);
            int bottomBlockY = (int) System.Math.Floor((Bottom - 1) / tileSize);
            int rightBlockX = (int) System.Math.Floor((Right - 1) / tileSize);
            
            //left upper 
            graphics.Draw(
                content.GetTexture("default"),
                new Rectangle(
                    (int) (leftBlockX * tileSize),
                    (int) ( topBlockY * tileSize), 
                    (int) tileSize, 
                    (int) tileSize),
                new Color(0, 100, 0, 0.2f));
            
            //left upper 
            graphics.Draw(
                content.GetTexture("default"),
                new Rectangle(
                    (int) (leftBlockX * tileSize),
                    (int) ( bottomBlockY * tileSize), 
                    (int) tileSize, 
                    (int) tileSize),
                new Color(0, 100, 0, 0.2f));
            
            //right lower 
            graphics.Draw(
                content.GetTexture("default"),
                new Rectangle(
                    (int) (rightBlockX * tileSize),
                    (int) ( bottomBlockY * tileSize), 
                    (int) tileSize, 
                    (int) tileSize),
                new Color(0, 100, 0, 0.2f));
            
            //right upper 
            graphics.Draw(
                content.GetTexture("default"),
                new Rectangle(
                    (int) (rightBlockX * tileSize),
                    (int) ( topBlockY * tileSize), 
                    (int) tileSize, 
                    (int) tileSize),
                new Color(0, 100, 0, 0.2f));

            //entity Bounds
            graphics.DrawBorder(new Rectangle((int)Position.X, (int)Position.Y,(int)Dimensions.X,(int)Dimensions.Y),1,Color.Black, content.GetTexture("default"));
            
            Draw(graphics, worldScale);

            //Position
            graphics.DrawBackedText($"X: {(int)Position.X}, Y: {(int)Position.Y}",new BfbVector(Position.X, Position.Y - 15), content, 0.2f * worldScale);
            
            //velocity vector
            graphics.DrawVector(new Vector2(Position.X + Dimensions.X/2, Position.Y + Dimensions.Y/2),Velocity.ToVector2()  * 4 * worldScale, 1, Color.Red, content);
            
            //orientation vector
            graphics.DrawLine(new Vector2(Position.X + Dimensions.X/2, Position.Y + 10),new Vector2((Position.X + Dimensions.X/2) + (Velocity.X < 0 ? -30 : 30), Position.Y + 10), 1, Color.Green, content);
            
        }

    }
}