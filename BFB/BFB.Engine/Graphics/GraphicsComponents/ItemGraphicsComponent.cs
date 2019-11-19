using BFB.Engine.Content;
using BFB.Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Simulation.GraphicsComponents
{
    public class ItemGraphicsComponent : IGraphicsComponent
    {
        private readonly float _rotationStep;
        private readonly int _offsetScale;
        private int _lifeTime;
        private float _yOffset;
        private readonly AtlasTexture _atlasTexture;
        private float _rotation;
        
        public ItemGraphicsComponent(AtlasTexture atlas)
        {
            _atlasTexture = atlas;

            _rotationStep = 0.01f;
            _yOffset = 0;
            _lifeTime = 0;
            _offsetScale = 5;
            _rotation = 0;

        }
        
        public void Update(ClientEntity entity)
        {
            _lifeTime++;

            _yOffset = (int)(System.Math.Sin(_lifeTime * 0.1f) * _offsetScale);
            _rotation += _rotationStep;
        }

        public void Draw(ClientEntity entity, SpriteBatch graphics, float scale = 1)
        {
            graphics.Draw(_atlasTexture.Texture,
                new Vector2(entity.Position.X + 15, entity.Position.Y + 15 + _yOffset),
                new Rectangle(_atlasTexture.X,_atlasTexture.Y,_atlasTexture.Width,_atlasTexture.Height),
                Color.White,
                _rotation,
                new Vector2(15/2f, 15/2f), 
                scale * 0.75f,
                SpriteEffects.None,
                1);
        }
    }
}