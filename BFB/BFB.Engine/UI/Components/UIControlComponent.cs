using System;
using System.Linq.Expressions;
using BFB.Engine.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI.Components
{
    public class UIControlComponent<TModel> : UIComponent
    {
        private readonly TModel _model;
        private readonly Expression<Func<TModel, Keys>> _valueSelector;

        private readonly Action<UIEvent, string> _clickAction;
        private readonly Action<UIEvent, string> _keyHandler;
        
        public UIControlComponent(TModel model, Expression<Func<TModel, Keys>> valueSelector, Action<UIEvent,string> clickHandler = null, Action<UIEvent,string> keyHandler = null) : base(nameof(UIControlComponent<TModel>))
        {
            _model = model;
            _valueSelector = valueSelector;

            Text = valueSelector.Compile()(model).ToString();
            
            this.Color(Color.Black)
                .Background(new Color(169, 170, 168))
                .Border(3, new Color(211, 212, 210));
            
            AddEvent("click", e =>
            {
            });
            
        }

//        public override void Render(SpriteBatch graphics, BFBContentManager content)
//        {
//            base.Render(graphics, content);
//            
//            
//        }
    }
}