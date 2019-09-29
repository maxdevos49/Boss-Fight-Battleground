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
                        tileMap.setTileBlock(x, y, (int)blocks.AIR);
                    }
                    else if (y < 6)
                    {
                        tileMap.setTileBlock(x, y, (int)blocks.GRASS);
                    }
                    else if(y < 80)
                    {
                        tileMap.setTileBlock(x, y, (int)blocks.DIRT);
                    }
                    else
                    {
                        tileMap.setTileBlock(x, y, (int)blocks.STONE);
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
        }
        #endregion

        public int scale { get; set; }
        public bool grow;

        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            int x = 0;
            int y = 0;
            //graphics.Draw(grassTexture, new Vector2(100, 100), new Rectangle(0, 0, 100, 100), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
            for (x = 0; x < widthX; x++)
            {
                for(y = 0; y < heightY; y++)
                {
                    switch(tileMap.getTileBlock(x, y))
                    {
                        case (int)blocks.GRASS:
                            graphics.Draw(grassTexture, new Vector2(x * scale, y * scale), Color.White);
                            break;
                        case (int)blocks.DIRT:
                            graphics.Draw(dirtTexture, new Vector2(x * scale, y * scale), Color.White);
                            break;
                        case (int)blocks.STONE:
                            graphics.Draw(stoneTexture, new Vector2(x * scale, y * scale), Color.White);
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
