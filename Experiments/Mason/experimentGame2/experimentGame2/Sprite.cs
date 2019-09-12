using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace experimentGame2
{
    public class Sprite
    {
        private Texture2D _texture;
        private float _rotation;

        public Vector2 Position;
        public Vector2 Origin;

        public float RotationVelocity = 3f;
        public float LinearVelocity = 4f;

        public Input Input;

        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                _rotation -= MathHelper.ToRadians(RotationVelocity);
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                _rotation += MathHelper.ToRadians(RotationVelocity);

            var direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Position += direction * LinearVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin, 1, SpriteEffects.None, 0f);
        }

        // This Move() method is for linear movement up, down, left, and right. No rotation involved.

        //private void Move()
        //{
        //    if (Input == null)
        //        return;

        //    if (Keyboard.GetState().IsKeyDown(Input.Up))
        //        Position.Y -= LinearVelocity;

        //    if (Keyboard.GetState().IsKeyDown(Input.Down))
        //        Position.Y += LinearVelocity;

        //    if (Keyboard.GetState().IsKeyDown(Input.Left))
        //        Position.X -= LinearVelocity;

        //    if (Keyboard.GetState().IsKeyDown(Input.Right))
        //        Position.X += LinearVelocity;
        //}
    }
}
