//c#
using System;
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Engine
using BFB.Engine.Scene;

namespace BFB.Client.Scenes
{
    public class ExampleScene : Scene
    {

        /**
         * Managers available for use by inheritence and dependency injection
         **/
        // _sceneManager;
        // _contentManager;
        // _graphicsManager;
        // _eventManager;

        private Vector2 MousePosition { get; set; }
        private Texture2D SpaceshipTexture { get; set; }

        private List<Spaceship> Spaceships;

        /**
         * Scenes require using there class name as a identifier key. "nameof(ClassName)" returns the class
         * name or any variable/class to a string. Also base() is the equivalant of super in C#.
         *
         * Only use the constructor for initializing default values or constant or persistent data
         * */
        public ExampleScene() : base(nameof(ExampleScene))
        {
            Spaceships = new List<Spaceship>();
        }

        #region Init

        /**
         * This is fired when a scene is newly started and should be used for initilizing values and not the
         * constructor. This is because the scene may be reset during the course of the game and the
         * constructor is only ever called once.
         * */
        public override void Init()
        {
            MousePosition = Vector2.Zero;

            Spaceships.Add(new Spaceship(SpaceshipTexture.Width, SpaceshipTexture.Height));

            AddEventListener("mousemove", (Event) =>
            {
                MousePosition = new Vector2(Event.Mouse.X, Event.Mouse.Y);
            });

            AddEventListener("mousedown", (Event) =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Spaceships.Add(new Spaceship(SpaceshipTexture.Width, SpaceshipTexture.Height, new Vector2(Event.Mouse.X, Event.Mouse.Y)));
                }
                Console.WriteLine($"Test, {Spaceships.Count}");
            });

        }

        #endregion

        #region Load

        /**
         * This is fired once when a scene is started/launched. Use it to load content needed for the scene only.
         * */
        public override void Load()
        {
            SpaceshipTexture = _contentManager.Load<Texture2D>("Sprites\\spaceship");
        }

        #endregion

        #region Update

        /**
         * Use this for updating coordinates/calculations
         * */
        public override void Update(GameTime gameTime)
        {
            foreach (var ship in Spaceships)
            {
                ship.Update(MousePosition);
            }
        }

        #endregion

        #region Draw

        /**
         * Use this for drawing only! If you use this for calculations this may not be called as
         * often as the update loop dependent on game performance.(aka FrameSkipping occurs)
         * */
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            foreach (var ship in Spaceships)
            {
                ship.Draw(SpaceshipTexture, graphics);
            }
        }

        #endregion


        /**
         * For example scene
         * */
        public class Spaceship
        {
            private Vector2 Position { get; set; }
            private Vector2 Velocity { get; set; }
            private Vector2 Origin { get; set; }
            private float MaxForce { get; set; }
            private float MaxSpeed { get; set; }
            private float Rotation { get; set; }
            private int Width { get; set; }
            private int Height { get; set; }

            public Spaceship(int width, int height, Vector2? initPos = null)
            {
                Width = width;
                Height = height;

                var rnd = new Random();

                MaxForce = (float)rnd.NextDouble();
                MaxSpeed = rnd.Next() * 5;
                Rotation = (float)rnd.NextDouble();
                Position = new Vector2(300, 300);
                Velocity = Vector2.Zero;
                Origin = new Vector2(Width / 2, Height / 2);
            }

            private void ApplySteering(Vector2 desiredVector)
            {
                //calculate steering vector
                var desMag = desiredVector.Length();
                desiredVector.X = desiredVector.X * MaxSpeed / desMag;
                desiredVector.Y = desiredVector.Y * MaxSpeed / desMag;
                var steering = Vector2.Subtract(desiredVector, Velocity);

                //enforce max force
                if (steering.Length() > MaxForce)
                {
                    var steerMag = steering.Length();
                    steering.X = steering.X * MaxForce / steerMag;
                    steering.Y = steering.Y * MaxForce / steerMag;
                }

                //Apply steering to velocity
                Velocity = Vector2.Add(steering, Velocity);

                //update position
                Position = Vector2.Add(Velocity, Position);

                //update Rotation with degrees
                Rotation = Convert.ToSingle(Math.Atan2(Velocity.Y, Velocity.X) - Math.PI / 2);
            }

            public void Update(Vector2 mousePos)
            {
                ApplySteering(Vector2.Subtract(mousePos, Position));
            }

            public void Draw(Texture2D texture, SpriteBatch graphics)
            {
                graphics.Draw(texture, Position, new Rectangle(0, 0, 100, 100), Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
            }
        }
    }
}
