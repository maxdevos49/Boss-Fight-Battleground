using BFB.Engine.Math;
using BFB.Engine.Simulation.GraphicsComponents;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Entity
{
    public class ClientEntity : Entity
    {
        #region Properties
        
        private readonly IGraphicsComponent _graphics;
        
        #endregion

        #region Constructor
        
        public ClientEntity(string entityId, EntityOptions options, IGraphicsComponent graphics) : base(entityId, options)
        {
            _graphics = graphics;
        }
        
        #endregion
        
        #region Update

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

        public void Draw(SpriteBatch graphics)
        {
            _graphics?.Draw(this, graphics);
        }

        #endregion

    }
}