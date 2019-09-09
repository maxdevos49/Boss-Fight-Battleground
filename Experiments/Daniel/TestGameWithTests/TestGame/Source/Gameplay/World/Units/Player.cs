using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;

namespace TestGame.Source.Gameplay.World
{
    public class Player : Unit
    {
        private Vector2 velocity;
        public Player(string PATH, Vector2 POS, Vector2 DIMS) : base(PATH, POS, DIMS)
        {
            velocity = new Vector2(0, 0);
        }

        public override void Update()
        {
            updateVelocity(this, Globals.keyboard);
            updatePosition(this);

            if(Globals.mouse.LeftClick())
            {
                GameGlobals.PassProjectile(new Fireball(new Vector2(pos.X, pos.Y), this, new Vector2(Globals.mouse.newMousePos.X, Globals.mouse.newMousePos.Y)));
            }
            base.Update();
        }

        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }

        public void updateVelocity(Player player, MyKeyboard keyboard)
        {
            player.velocity = updateVelWithFriction(player);
            player.velocity = updateVelWithInput(player, keyboard);
        }

        public Vector2 updateVelWithFriction(Player player)
        {
            Vector2 newVelocity = new Vector2(player.velocity.X, player.velocity.Y);

            if (newVelocity.Y > 0)
                newVelocity.Y -= 0.5f;
            if (newVelocity.Y < 0)
                newVelocity.Y += 0.5f;
            if (newVelocity.X > 0)
                newVelocity.X -= 0.25f;
            if (newVelocity.X < 0)
                newVelocity.X += 0.25f;

            return newVelocity;
        }

        public Vector2 updateVelWithInput(Player player, MyKeyboard keyboard)
        {
            Vector2 newVelocity = new Vector2(player.velocity.X, player.velocity.Y);

            if (keyboard.GetPress("W") && !isInAir)
            {
                newVelocity.Y = -12;
                player.isInAir = true;
            }
            if (keyboard.GetPress("S"))
                newVelocity.Y = 12;
            if (keyboard.GetPress("A"))
                newVelocity.X = -4;
            if (keyboard.GetPress("D"))
                newVelocity.X = 4;

            return newVelocity;
        }

        public void updatePosition(Player player)
        {
            Vector2 newPosition = new Vector2(player.velocity.X + player.pos.X, player.velocity.Y + player.pos.Y + Globals.world.gravity.Y);

            if (newPosition.Y < 0 + player.dims.Y / 2)
                newPosition.Y = 0 + player.dims.Y / 2;
            if (newPosition.Y > Globals.screenHeight - player.dims.Y / 2)
            {
                newPosition.Y = Globals.screenHeight - player.dims.Y / 2;
                player.isInAir = false;
            }
            if (newPosition.X < 0 + player.dims.X / 2)
                newPosition.X = 0 + player.dims.X / 2;
            if (newPosition.X > Globals.screenWidth - player.dims.X / 2)
                newPosition.X = Globals.screenWidth - player.dims.X / 2;

            player.pos = newPosition;
        }
        
    }


}
