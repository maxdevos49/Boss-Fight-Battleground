using BFB.Engine.Entity;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Simulation.GraphicsComponents
{
    /// <summary>
    /// The interface that outlines how to draw an entity.
    ///
    /// Note: This is only ever used on a client
    /// </summary>
    public interface IGraphicsComponent
    {
        /// <summary>
        /// Called for every frame the entity is ticked
        /// </summary>
        /// <param name="entity">The given entity</param>
        void Update(ClientEntity entity);

        /// <summary>
        /// Called whenever the entity is drawn
        /// </summary>
        /// <param name="entity">The given entity</param>
        /// <param name="graphics">The spritbatch object used for drawing</param>
        void Draw(ClientEntity entity, SpriteBatch graphics);
    }
}