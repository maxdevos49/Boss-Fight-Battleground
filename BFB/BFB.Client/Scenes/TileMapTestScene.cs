//c#
using System;
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Engine
using BFB.Engine.Scene;
using BFB.Engine.TileMap;


namespace BFB.Client.Scenes
{
    public class TileMapTestScene :  Scene
    {
        private const int heightY = 320;
        private const int widthX = 480;
        
        private Texture2D grassTexture;
        private Texture2D dirtTexture;
        private Texture2D stoneTexture;

        private int randNum;
        private Random random = new Random();

        public enum blocks {
            AIR = 0,
            GRASS,
            DIRT,
            STONE
        }

        private TileMapManager tileMap;

        public TileMapTestScene() : base(nameof(TileMapTestScene))
        {
            tileMap = new TileMapManager();
            scale = 15;
            grow = true;
            offset = 0;
        }

        #region Init

        public override void Init()
        {
            int x;
            int y;

            for(x = 0; x < widthX; x++)
            {
                for(y = 0; y < heightY; y++)
                {
                    if(y < 5)
                    {
                        tileMap.setBlock(x, y, (int)blocks.AIR);
                    }
                    else if (y < 12)
                    {
                        if (tileMap.getBlock(x, y - 1) == (int)blocks.AIR)
                        {
                            randNum = random.Next(3);
                            if (randNum == 1)
                            {
                                tileMap.setBlock(x, y, (int)blocks.GRASS);
                            }
                            else
                            {
                                tileMap.setBlock(x, y, (int)blocks.AIR);
                            }
                            if(y == 11)
                            {
                                tileMap.setBlock(x, y, (int)blocks.GRASS);
                            }
                        }
                        else if(tileMap.getBlock(x, y - 1) == (int)blocks.GRASS || tileMap.getBlock(x, y - 1) == (int)blocks.DIRT)
                        {
                            tileMap.setBlock(x, y, (int)blocks.DIRT);
                        }
                    }
                    else if(y < 25)
                    {
                        tileMap.setBlock(x, y, (int)blocks.DIRT);
                    }

                    else if (y < 30)
                    {
                        if (tileMap.getBlock(x, y - 1) == (int)blocks.STONE)
                        {
                            tileMap.setBlock(x, y, (int)blocks.STONE);
                        }
                        else
                        {
                            randNum = random.Next(3);
                            if (randNum == 1)
                            {
                                tileMap.setBlock(x, y, (int)blocks.STONE);
                            }
                            else
                            {
                                tileMap.setBlock(x, y, (int)blocks.DIRT);
                            }
                            if (y == 29)
                            {
                                tileMap.setBlock(x, y, (int)blocks.STONE);
                            }
                        }

                    }

                    else
                    {
                        tileMap.setBlock(x, y, (int)blocks.STONE);
                    }
                }
            }
        }
        #endregion

        #region Load
        public override void Load()
        {
            grassTexture = _contentManager.Load<Texture2D>("Sprites\\grass");
            dirtTexture = _contentManager.Load<Texture2D>("Sprites\\dirt");
            stoneTexture = _contentManager.Load<Texture2D>("Sprites\\stone");
        }
        #endregion

        #region Unload
        public override void Unload()
        {

        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {

            /*
                if (grow){
                    scale++;
                }
                else
                {
                    scale--;
                }
                if(scale > 30)
                {
                    grow = false;
                }
                if(scale < 15)
                {
                    grow = true;
                }
            */
            if (grow)
            {
                offset+=1;
            }
            else
            {
                offset-=1;
            }
            if(offset > 6400)
            {
                grow = false;
            }
            if(offset <= 0)
            {
                grow = true;
            }
        }
        #endregion

        public int scale { get; set; }
        public int offset;
        public bool grow;

        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            int x = 0;
            int y = 0;
            for (x = 0; x < widthX; x++)
            {
                for(y = 0; y < heightY; y++)
                {
                    switch(tileMap.getBlock(x, y))
                    {
                        case (int)blocks.GRASS:
                            graphics.Draw(grassTexture, new Vector2(x * scale - offset, y * scale), Color.White);
                            break;
                        case (int)blocks.DIRT:
                            graphics.Draw(dirtTexture, new Vector2(x * scale - offset, y * scale), Color.White);
                            break;
                        case (int)blocks.STONE:
                            graphics.Draw(stoneTexture, new Vector2(x * scale - offset, y * scale), Color.White);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
