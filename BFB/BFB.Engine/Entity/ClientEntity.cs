using BFB.Engine.Entity.GraphicsComponents;
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