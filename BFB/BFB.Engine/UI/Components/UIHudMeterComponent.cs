using System;
using System.Linq.Expressions;
using BFB.Client;
using BFB.Engine.Content;
using BFB.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIHudMeterComponent : UIComponent
    {
        private ClientDataRegistry _model;
        private Func<ClientDataRegistry, ushort?> _valueSelector;
        private bool _percentMode;
        private bool _mode;
        private string textureKey;
        
        public UIHudMeterComponent(ClientDataRegistry model, Expression<Func<ClientDataRegistry,ushort?>> valueSelector, bool percentMode = false, bool mode = false) : base(nameof(UIHudMeterComponent), true)
        {
            _model = model;
            _valueSelector = valueSelector.Compile();
            _percentMode = percentMode;
            _mode = mode;

            if (mode)
                textureKey = "Mana";
            else
                textureKey = "Heart";
        }

        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            base.Render(graphics, content);
            
            if(_model?.Client == null || _valueSelector == null)
                return;

            Vector2 position = new Vector2(RenderAttributes.X, RenderAttributes.Y);
            int width = RenderAttributes.Width /10;
            int dimension = width - 5;

            ushort? valuep;
            try
            {
                valuep = _valueSelector(_model);
            }
            catch(Exception e)
            {
                return;
            }

            if (valuep == null)
                return;

            var value = (ushort)valuep;
            
            float percent = 20f / value;
            
            for (int i = 0; i < 10; i++)
            {
                string atlasKey;

                if (_percentMode == false)
                {
                    if (value - i * 2 >= 2)
                        atlasKey = textureKey + ":Full";
                    else if (value - i * 2 >= 1)
                        atlasKey = textureKey + ":Half";
                    else
                        atlasKey = textureKey + ":Empty";
                }
                else
                {
                    if (value - i*100 >= 100)
                        atlasKey = textureKey + ":Full";
                    else if (value - i * 50 >= 50)
                        atlasKey = textureKey + ":Half";
                    else
                        atlasKey = textureKey + ":Empty";
                }


                graphics.DrawAtlas(content.GetAtlasTexture(atlasKey), new Rectangle((int)position.X,(int)position.Y,dimension,dimension), Color.White);

                position.X += width;
            }
        }

    }

}