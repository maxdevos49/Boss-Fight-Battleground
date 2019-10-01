using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Entity.Components.Graphics
{
    public interface IGraphicsComponent
    {
        void Update(ClientEntity entity);

        void Draw(ClientEntity entity, SpriteBatch graphics);
    }
}