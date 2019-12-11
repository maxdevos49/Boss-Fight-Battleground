using System;
using System.Linq.Expressions;
using BFB.Engine.Event;
using BFB.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI.Components
{
    public class UICheckboxComponent<TModel> : UIComponent
    {
        private readonly TModel _model;
        private readonly Expression<Func<TModel, bool>> _valueSelector;
        
        public UICheckboxComponent(TModel model, Expression<Func<TModel,bool>> valueSelector) : base(nameof(UICheckboxComponent<TModel>), true)
        {
            Focusable = true;
            _model = model;
            _valueSelector = valueSelector;

            this.Color(Color.Black)
                .Background(new Color(169,170,168))
                .Border(3, new Color(211,212,210))
                .FontScaleMode(FontScaleMode.ContainerFitScale)
                .AspectRatio(1);
            
            AddEvent("keypress", HandleKeyPress);
            AddEvent("click", HandleMouseClick);
            AddEvent("hover", HandleHoverEvent);
        }
        
        #region HandleMouseCilck
        
        private void HandleMouseClick(UIEvent e)
        {
            _model.SetPropertyValue(_valueSelector, !_valueSelector.Compile()(_model));
        }
        
        #endregion
        
        #region HandleKeyPress

        private void HandleKeyPress(UIEvent e)
        {
            if(e.Keyboard.KeyEnum == Keys.Enter)
                HandleMouseClick(e);
        }
        
        #endregion
        
        #region HandleHoverEvent

        private void HandleHoverEvent(UIEvent e)
        {
            UIAttributes attr = new UIAttributes
            {
                Background = new Color(125, 125, 125),
                Color = Color.White
            };
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }
        
        #endregion

        #region Update
        
        public override void Update(GameTime time)
        {
            Text = _valueSelector.Compile()(_model) ? "X" : " ";
        }
        
        #endregion
    }
}