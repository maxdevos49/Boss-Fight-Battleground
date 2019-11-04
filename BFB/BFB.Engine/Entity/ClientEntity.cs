using BFB.Engine.Simulation.GraphicsComponents;
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
        }
        
        #endregion

        #region Draw

        /// <summary>
        /// Extends Entity's Update() method
        /// Updates this ClientEntity's information
        /// </summary>
        /// <param name="graphics">The SpriteBatch that this entity is a part of</param>
        public void Draw(SpriteBatch graphics)
        {
            _graphics?.Draw(this, graphics);
        }

        #endregion

    }
}