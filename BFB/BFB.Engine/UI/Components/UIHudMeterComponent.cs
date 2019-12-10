using System;
using System.Linq.Expressions;
using BFB.Engine.Content;
using BFB.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace BFB.Engine.UI.Components
{
    public class UIHudMeterComponent<TModel> : UIComponent
    {
        private TModel _model;
        private Func<TModel, ushort?> _valueSelector;
        private bool _percentMode;
        private bool _mode;
        private string textureKey;
        
        public UIHudMeterComponent(TModel model, Expression<Func<TModel,ushort?>> valueSelector, bool percentMode = false, bool mode = false) : base(nameof(UIHudMeterComponent<TModel>), true)
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
            
            if(_model == null)
                return;

            Vector2 position = new Vector2(RenderAttributes.X, RenderAttributes.Y);
            int width = RenderAttributes.Width /10;
            int dimension = width - 5;
            
            ushort? valuep = _valueSelector(_model);

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
                    if (1000 - value >= 2)
                        atlasKey = textureKey + ":Full";
                    else if (value - i * 2 >= 1)
                        atlasKey = textureKey + ":Half";
                    else
                        atlasKey = textureKey + ":Empty";
                }


                graphics.DrawAtlas(content.GetAtlasTexture(atlasKey), new Rectangle((int)position.X,(int)position.Y,dimension,dimension), Color.White);

                position.X += width;
            }
        }

        private void DrawItem(MeterSection section, Vector2 position)
        {
        }
    }

    public enum MeterSection
    {
        Empty,
        Half,
        Full
    }
}