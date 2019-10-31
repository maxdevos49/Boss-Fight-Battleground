////c#
//using System;
//using System.Collections.Generic;
//
////Monogame
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//
////Engine
//using BFB.Engine.Scene;
//using BFB.Engine.TileMap;
//
//
//namespace BFB.Client.Scenes
//{
//    public class TileMapTestScene :  Scene
//    {
//        private const int heightY = 320;
//        private const int widthX = 480;
//        
//        private Texture2D grassTexture;
//        private Texture2D dirtTexture;
//        private Texture2D stoneTexture;
//
//        private int randNum;
//        private Random random = new Random();
//
//        public enum blocks {
//            AIR = 0,
//            GRASS,
//            DIRT,
//            STONE
//        }
//
//        private WorldManager<> _world;
//
//        public TileMapTestScene() : base(nameof(TileMapTestScene))
//        {
//            _world = new WorldManager<>();
//            scale = 15;
//            direction = true;
//            offset = 0;
//        }
//
//        #region Init
//
//        protected override void Init()
//        {
//
//            for(int x = 0; x < widthX; x++)
//            {
//                for(int y = 0; y < heightY; y++)
//                {
//                    if(y < 5)
//                    {
//                        _world.SetBlock(x, y, (int)blocks.AIR);
//                    }
//                    else if (y < 12)
//                    {
//                        if (_world.GetBlock(x, y - 1) == (int)blocks.AIR)
//                        {
//                            randNum = random.Next(3);
//                            if (randNum == 1)
//                            {
//                                _world.SetBlock(x, y, (int)blocks.GRASS);
//                            }
//                            else
//                            {
//                                _world.SetBlock(x, y, (int)blocks.AIR);
//                            }
//                            if(y == 11)
//                            {
//                                _world.SetBlock(x, y, (int)blocks.GRASS);
//                            }
//                        }
//                        else if(_world.GetBlock(x, y - 1) == (int)blocks.GRASS || _world.GetBlock(x, y - 1) == (int)blocks.DIRT)
//                        {
//                            _world.SetBlock(x, y, (int)blocks.DIRT);
//                        }
//                    }
//                    else if(y < 25)
//                    {
//                        _world.SetBlock(x, y, (int)blocks.DIRT);
//                    }
//
//                    else if (y < 30)
//                    {
//                        if (_world.GetBlock(x, y - 1) == (int)blocks.STONE)
//                        {
//                            _world.SetBlock(x, y, (int)blocks.STONE);
//                        }
//                        else
//                        {
//                            randNum = random.Next(3);
//                            if (randNum == 1)
//                            {
//                                _world.SetBlock(x, y, (int)blocks.STONE);
//                            }
//                            else
//                            {
//                                _world.SetBlock(x, y, (int)blocks.DIRT);
//                            }
//                            if (y == 29)
//                            {
//                                _world.SetBlock(x, y, (int)blocks.STONE);
//                            }
//                        }
//
//                    }
//
//                    else
//                    {
//                        _world.SetBlock(x, y, (int)blocks.STONE);
//                    }
//                }
//            }
//        }
//        #endregion
//
//
//
//        #region Update
//        public override void Update(GameTime gameTime)
//        {
//            if (direction)
//            {
//                offset+=1;
//            }
//            else
//            {
//                offset-=1;
//            }
//            
//            
//            if(offset > 6400)
//            {
//                direction = false;
//            }
//            if(offset <= 0)
//            {
//                direction = true;
//            }
//        }
//        #endregion
//
//        public int scale { get; set; }
//        public int offset;
//        public bool direction;
//
//        #region Draw
//        public override void Draw(GameTime gameTime, SpriteBatch graphics)
//        {
//            for (int x = 0; x < widthX; x++)
//            {
//                for(int y = 0; y < heightY; y++)
//                {
//                    switch(_world.GetBlock(x, y))
//                    {
//                        case (int)blocks.GRASS:
//                            graphics.Draw(grassTexture, new Vector2(x * scale - offset, y * scale), Color.White);
//                            break;
//                        case (int)blocks.DIRT:
//                            graphics.Draw(dirtTexture, new Vector2(x * scale - offset, y * scale), Color.White);
//                            break;
//                        case (int)blocks.STONE:
//                            graphics.Draw(stoneTexture, new Vector2(x * scale - offset, y * scale), Color.White);
//                            break;
//                        default:
//                            break;
//                    }
//                }
//            }
//        }
//        #endregion
//    }
//}
