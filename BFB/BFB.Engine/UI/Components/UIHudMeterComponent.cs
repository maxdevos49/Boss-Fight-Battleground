using System;
using System.Linq.Expressions;
using System.Numerics;
using BFB.Engine.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIHudMeterComponent<TModel> : UIComponent
    {
        private TModel _model;
        private Func<TModel, ushort> _valueSelector;
        private bool _percentMode;
        private bool _mode;
        
        public UIHudMeterComponent(TModel model, Expression<Func<TModel,ushort>> valueSelector, bool percentMode = false, bool mode = false) : base(nameof(UIHudMeterComponent<TModel>), true)
        {
            _model = model;
            _valueSelector = valueSelector.Compile();
            _percentMode = percentMode;
            _mode = mode;
        }

        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            base.Render(graphics, content);

            if (_mode)
            {
                
            }
            else
            {
                
            }
        }

        private void DrawItem(MeterSection section, Vector2 position)
        {
            if (_mode)
            {
                
            }
            else
            {
                
            }
        }
    }

    public enum MeterSection
    {
        Empty,
        Half,
        Full
    }
}