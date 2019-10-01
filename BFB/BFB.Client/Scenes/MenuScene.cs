using System;
using System.Net.Mime;
using System.Numerics;
using BFB.Engine.Event;
using BFB.Engine.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace BFB.Client.Scenes
{
    public class MenuScene : Scene
    {
        private Texture2D _rec;
        private SpriteFont _font;
        private Button _b1;
        private Button _b2;
        
        public MenuScene(): base(nameof(MenuScene))
        { }

        protected override void Init()
        {
            _b1 = new Button(Vector2.One*120, new Vector2(100,30), "Connection Scene", EventManager);
            
            _b1.Onclick(() =>
            {
                SceneManager.StartScene(nameof(ConnectionScene));
            });
            
            _b2 = new Button(new Vector2(120,160), new Vector2(100,30), "Non Connected Spaceships", EventManager);
            
            _b2.Onclick(() =>
            {
                SceneManager.StartScene(nameof(ExampleScene));
            });
            
        }
        
        protected override void Load()
        {
            _font = ContentManager.Load<SpriteFont>("Fonts\\Papyrus");
            _rec = new Texture2D(GraphicsDeviceManager.GraphicsDevice, 1,1);
            _rec.SetData(new[] { Color.White });//Fills it white. Can be tinted
        }
        
        protected override void Unload()
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            graphics.DrawString(_font, "Boss Fight Battlegrounds", new Vector2(100,50), Color.Black,0,Vector2.Zero,3.0f,SpriteEffects.None,1);

            _b1.Draw(graphics,_font);
            _b2.Draw(graphics,_font);
            base.Draw(gameTime, graphics);
        }
    }

    public class Button
    {

        private string _text;
        
        private Vector2 _position;
        private Vector2 _dimensions;

        private Action _onHover;
        private Action _onClick;
        private int _offset;
        
        public Button(Vector2 position, Vector2 dimensions, string text, EventManager events)
        {
            _position = position;
            _dimensions = dimensions;
            _text = text;

            _onClick = () => Console.WriteLine("Clicked!");
            _onHover = () => Console.WriteLine("Hover");

            events.AddEventListener("mousemove", (e) =>
            {
                //Is mouse over
                if (e.Mouse.X > _position.X && e.Mouse.Y > _position.Y && e.Mouse.X < _position.X + _dimensions.X &&
                    e.Mouse.Y < _position.Y + _dimensions.Y)
                {
                    _offset = 10;
                    _onHover();
                }
                else
                {
                    _offset = 0;
                }
            });
            
            events.AddEventListener("mouseclick", (e) =>
            {
                //Is mouse over
                if (e.Mouse.X > _position.X && e.Mouse.Y > _position.Y && e.Mouse.X < _position.X + _dimensions.X &&
                    e.Mouse.Y < _position.Y + _dimensions.Y)
                {
                    _onClick();
                }
            });

        }

        public void OnHover(Action handler)
        {
            _onHover = handler;
        }

        public void Onclick(Action handler)
        {
            _onClick = handler;
        }

        public void Draw(SpriteBatch graphics, SpriteFont font)
        {
            //Font
            graphics.DrawString(font, _text, new Vector2(_position.X + _offset,_position.Y), Color.Black);
        }
    }
}