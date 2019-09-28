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
        
        private texture2D grass;
        private texture2D dirt;
        private texture2D stone;

        private enum blocks {
            AIR = 0,
            GRASS,
            DIRT,
            STONE
        }

        private TileMapManager tileMap;

        public TileMapTestScene() : base(nameof(TileMapTestScene))
        {
            tileMap = new TileMapManager();
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
                    if(y < 280)
                    {

                    }
                    else if(y < 300)
                    {

                    }
                }
            }
        }
        #endregion

        #region Load
        public override void Load()
        {
            grass = _contentManager.Load<Texture2D>("Sprites\\grass");
            dirt = _contentManager.Load<Texture2D>("Sprites\\dirt");
            stone = _contentManager.Load<Texture2D>("Sprites\\stone");
        }
        #endregion

        #region Unload
        public override void Unload()
        {

        }
        #endregion

        #region Update
        public override void Update()
        {

        }
        #endregion

        #region Draw
        public override void Draw()
        {

        }
        #endregion
    }
}
