using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;
using TestGame.Source.Gameplay.World;

namespace TestGame.Source
{
    public class World
    {
        public Player player;
        public List<Projectile2d> projectiles = new List<Projectile2d>();

        public Background bg;

        public Vector2 offset;
        public Vector2 gravity;

        public World()
        {
            player = new Player("2d\\stickman-test-2", 
                new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), 
                new Vector2(60, 60));
            bg = new Background("2d\\sf-background",
                new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2),
                new Vector2(Globals.screenWidth, Globals.screenHeight));
            gravity = new Vector2(0, 5);
            offset = new Vector2(0, 0);

            GameGlobals.PassProjectile = AddProjectile;
        }

        public virtual void Update()
        {
            bg.Update();
            player.Update();

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(offset, null);
                if (projectiles[i].done)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public virtual void AddProjectile(object INFO)
        {
            projectiles.Add((Projectile2d)INFO);
        }

        public virtual void Draw(Vector2 OFFSET)
        {
            bg.Draw(OFFSET);
            player.Draw(OFFSET);

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(offset);
            }
        }
    }
}
