using System;
using System.Linq.Expressions;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIHudMeterComponent<TModel> : UIComponent
    {
        private readonly TModel _model;
        private readonly Expression<Func<TModel, EntityMeta>> _valueSelector;
        private readonly string _textureKey;
        
        public UIHudMeterComponent(TModel model, Expression<Func<TModel, EntityMeta>> valueSelector, bool mode = false) : base(nameof(UIHudMeterComponent<TModel>), true)
        {
            _model = model;
            _valueSelector = valueSelector;
            _textureKey = mode ? "Mana" : "Heart";
        }

        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            base.Render(graphics, content);

            EntityMeta meta = _valueSelector.Compile()?.Invoke(_model);
            
            if(meta == null)
                return;

            Vector2 cursor = new Vector2(RenderAttributes.X, RenderAttributes.Y);
            int width = RenderAttributes.Width /10;
            int dimension = width - 5;

            ushort value = _textureKey == "Mana" ? meta.Mana : meta.Health;
            ushort valueStep = _textureKey == "Mana" ? (ushort)(meta.MaxMana/10) : (ushort)(meta.MaxHealth/10);

            for (int i = 0; i < 10; i++)
            {
                string atlasKey;
                
                if (value - i * valueStep >= valueStep)
                    atlasKey = _textureKey + ":Full";
                else if (value - i * valueStep >= valueStep * 0.75f)
                    atlasKey = _textureKey + ":Most";
                else if (value - i * valueStep >= valueStep * 0.5f)
                    atlasKey = _textureKey + ":Half";
                else if (value - i * valueStep >= valueStep * 0.25f)
                    atlasKey = _textureKey + ":Partial";
                else
                    atlasKey = _textureKey + ":Empty";

                graphics.DrawAtlas(content.GetAtlasTexture(atlasKey), new Rectangle((int)cursor.X,(int)cursor.Y,dimension,dimension), Color.White);

                cursor.X += width;
            }
        }

    }

}