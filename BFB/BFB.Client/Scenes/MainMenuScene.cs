using System;
using BFB.Client.UI;
using BFB.Engine.Scene;
using BFB.Engine.TileMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class MainMenuScene : Scene
    {
        
        private const int HeightY = 320;
        private const int WidthX = 480;

        private readonly Random _random;

        private readonly int _scale;
        private int _offset;
        private bool _grow;

        
//        private readonly WorldManager<> _world;

        public MainMenuScene() : base(nameof(MainMenuScene))
        {
//            _world = new WorldManager<>();
            _random = new Random();
            _scale = 15;
            _grow = true;
            _offset = 0;
            
           
        }

        protected override void Init()
        {
            UIManager.Start(nameof(MainMenuUI));

//            for (int x = 0; x < WidthX; x++)
//            {
//                for (int y = 0; y < HeightY; y++)
//                {
//                    
//                    if(y < 11)
//                    {
//                        _world.SetBlock(x, y, (int)Blocks.Air);
//                    }
//                    else if (y < 12)
//                    {
//                        _world.SetBlock(x, y, (int)Blocks.Grass);
//                    }
//                    else if (y < 16)
//                    {
//                        _world.SetBlock(x, y, (int)Blocks.Dirt);
//                    }
//                    else if (y < 25)
//                    {
//                        if (_random.Next(y) + 2 > 16)
//                        {
//                            _world.SetBlock(x,y,(int)Blocks.Stone);
//                        }
//                        else
//                        {
//                            _world.SetBlock(x, y, (int) Blocks.Dirt);
//                        }
//
//                    }
//                    else
//                    {
//                        _world.SetBlock(x, y, (int)Blocks.Stone);
//                    }
//                }
//            }
        }
        
        protected override void Load()
        {
          
        }
        
        #region Update
        public override void Update(GameTime gameTime)
        {
            if (_grow)
            {
                _offset+=1;
            }
            else
            {
                _offset-=1;
            }
            if(_offset > 6400)
            {
                _grow = false;
            }
            if(_offset <= 0)
            {
                _grow = true;
            }
        }
        #endregion
        


        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
//            int height = 450;
//            int width = 800;
//
//            int xTile = _offset/_scale;
//            int yTile = 0;
//            int widthTile = (_offset / _scale + width / _scale) + 2;
//            int heightTile = 50;
//            
//            
//            for (int x = xTile; x < widthTile; x++)
//            {
//                for(int y = yTile; y < heightTile; y++)
//                {
//                    switch(_world.GetBlock(x, y))
//                    {
//                        case (int)Blocks.Grass:
//                            graphics.Draw(ContentManager.GetTexture("grass"), new Vector2(x * _scale - _offset, y * _scale), Color.White);
//                            break;
//                        case (int)Blocks.Dirt:
//                            graphics.Draw(ContentManager.GetTexture("dirt"), new Vector2(x * _scale - _offset, y * _scale), Color.White);
//                            break;
//                        case (int)Blocks.Stone:
//                            graphics.Draw(ContentManager.GetTexture("stone"), new Vector2(x * _scale - _offset, y * _scale), Color.White);
//                            break;
//                    }
//                }
//            }
        }
        #endregion

    }
    
   
}