using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Entity.GraphicsComponents
{
    public interface IGraphicsComponent
    {
        void Update(ClientEntity entity);

        void Draw(ClientEntity entity, SpriteBatch graphics);
    }
}