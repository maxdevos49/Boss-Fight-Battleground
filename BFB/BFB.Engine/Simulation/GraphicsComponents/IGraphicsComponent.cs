using BFB.Engine.Entity;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Simulation.GraphicsComponents
{
    public interface IGraphicsComponent
    {
        void Update(ClientEntity entity);

        void Draw(ClientEntity entity, SpriteBatch graphics, float scale = 1);
    }
}