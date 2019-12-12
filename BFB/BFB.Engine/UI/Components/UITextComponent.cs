using System;
using BFB.Engine.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UITextComponent<TModel> : UIComponent
    {
        #region Properties
        
        private readonly TModel _model;
        private readonly Func<TModel, string> _propertySelector;
        
        #endregion
        
        #region Constuctors
        
        public UITextComponent(TModel model, Func<TModel,string> propertySelector) : base(nameof(UITextComponent<TModel>))
        {
            _model = model;
            _propertySelector = propertySelector;
        }
        
        
        public UITextComponent(string text) : base(nameof(UITextComponent<TModel>))
        {
            Text = text;
            _propertySelector = null;
        }
        
        #endregion

        #region Render
        
        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            Text = _propertySelector == null ? Text : _propertySelector(_model);
            
            base.Render(graphics, content);
        }
        
        #endregion
 
    }
}