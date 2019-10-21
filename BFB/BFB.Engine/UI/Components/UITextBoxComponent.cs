using System;
using System.Linq.Expressions;
using System.Reflection;
using BFB.Engine.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI.Components
{
    public class UITextBoxComponent<TModel> : UIComponent
    {
        
        #region Properties
        
        private readonly TModel _model;
        private readonly Expression<Func<TModel, string>> _selector;

        private Action<UIEvent, UIComponentAttributes> _clickAction;
        private Action<UIEvent, UIComponentAttributes> _focusAction;
        private Action<UIEvent, UIComponentAttributes> _keyPressAction;
        private Action<UIEvent, UIComponentAttributes> _hoverAction;

        private Keys _previousKey;
        private int _keyRepeatCounter;
        
        #endregion
        
        #region Constructor
        
        public UITextBoxComponent(
            TModel model, 
            Expression<Func<TModel,string>> selector, 
            Action<UIEvent,UIComponentAttributes> clickAction = null,
            Action<UIEvent,UIComponentAttributes> focusAction = null,
            Action<UIEvent,UIComponentAttributes> keyPressAction = null,
            Action<UIEvent,UIComponentAttributes> hoverAction = null) : base(nameof(UITextBoxComponent<TModel>))
        {
            _model = model;
            _selector = selector;

            _clickAction = clickAction;
            _focusAction = focusAction;
            _keyPressAction = keyPressAction;
            _hoverAction = hoverAction;

            Focusable = true;
            
            AddEvent("keypress", KeyPressEvent);
            AddEvent("keydown", KeyDownEvent);
            AddEvent("hover", HoverEvent);
            AddEvent("click", ClickEvent);
            
            AddEvent("keyup", e =>
            {
                _keyRepeatCounter = 0;
            });
      
            
        }
        
        #endregion
        
        #region KeyPressEvent

        private void KeyPressEvent(UIEvent e)
        {
            //mark key pressed
            _previousKey = e.Keyboard.KeyEnum;
            _keyRepeatCounter = 0;
                
            ProcessKeys(e);
                
            UIComponentAttributes attr = new UIComponentAttributes();
            _keyPressAction?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }
        
        #endregion
        
        #region KeyDownEvent

        private void KeyDownEvent(UIEvent e)
        {
            if (_previousKey == e.Keyboard.KeyEnum)
                _keyRepeatCounter++;

            if (_keyRepeatCounter > 30)
                ProcessKeys(e);
                  
        }
        
        #endregion
        
        #region HoverEvent

        private void HoverEvent(UIEvent e)
        {
            UIComponentAttributes attr = new UIComponentAttributes();
            _hoverAction?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }     
        
        #endregion
        
        #region ClickEvent

        private void ClickEvent(UIEvent e)
        {
            UIComponentAttributes attr = new UIComponentAttributes();
            _clickAction?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }     
        
        #endregion
        
        #region ProcessKeys
        
        public void ProcessKeys(UIEvent e)
        {
            string text = _selector.Compile().Invoke(_model);
                
            switch (e.Keyboard.KeyEnum)
            {
                case Keys.Back when text.Length > 0:
                    text = text.Substring(0, text.Length - 1);
                    break;
                case Keys.Space:
                    text += " ";
                    break;
                default:
                {
                    if(e.Keyboard.KeyEnum.ToString().Length == 1)
                        if (e.Keyboard.KeyboardState.IsKeyDown(Keys.LeftShift) || e.Keyboard.KeyboardState.IsKeyDown(Keys.RightShift)) //if shift is held down
                            text += e.Keyboard.KeyEnum.ToString().ToUpper(); //convert the Key enum member to uppercase string
                        else
                            text += e.Keyboard.KeyEnum.ToString().ToLower(); //convert the Key enum member to lowercase string
                    break;
                }
            }
                
            _model.SetPropertyValue(_selector,text);
        }
        
        #endregion
        
        #region Render
        
        public override void Render(SpriteBatch graphics, Texture2D texture, SpriteFont font)
        {
            base.Render(graphics, texture, font);

            string text =  _selector.Compile().Invoke(_model);
            
//            (float x, float y) = font.MeasureString(text);
            graphics.DrawString(font, text, new Vector2(RenderAttributes.X + RenderAttributes.Width/2,RenderAttributes.Y + RenderAttributes.Height/2) , RenderAttributes.Color,0,new Vector2(RenderAttributes.Width/2,RenderAttributes.Height/2), RenderAttributes.FontSize,SpriteEffects.None,1);
            
//            DrawString(graphics, font, text, new Rectangle(RenderAttributes.X,RenderAttributes.Y,RenderAttributes.Width ,RenderAttributes.Height ));
        }
        
        #endregion
    }
    
    //TODO move to own file in a helpers folder??
    public static class LambdaExtensions
    {
        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLambda, TValue value)
        {
            if (!(memberLambda.Body is MemberExpression memberSelectorExpression)) return;
            
            PropertyInfo property = memberSelectorExpression.Member as PropertyInfo;
            
            if (property != null)
            {
                property.SetValue(target, value, null);
            }
        }
    }
}