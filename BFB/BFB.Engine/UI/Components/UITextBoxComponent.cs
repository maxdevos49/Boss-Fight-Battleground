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

        private readonly Action<UIEvent, UIComponentAttributes> _clickAction;
        private readonly Action<UIEvent, UIComponentAttributes> _keyPressAction;
        private readonly Action<UIEvent, UIComponentAttributes> _hoverAction;

        private Keys _previousKey;
        private int _keyRepeatCounter;
        private int _tick;
        private bool _showCursor;
        
        #endregion
        
        #region Constructor
        
        public UITextBoxComponent(
            TModel model, 
            Expression<Func<TModel,string>> selector, 
            Action<UIEvent,UIComponentAttributes> clickAction = null,
            Action<UIEvent,UIComponentAttributes> keyPressAction = null,
            Action<UIEvent,UIComponentAttributes> hoverAction = null) : base(nameof(UITextBoxComponent<TModel>))
        {
            Focusable = true;
            
            _model = model;
            _selector = selector;

            _clickAction = clickAction;
            _keyPressAction = keyPressAction;
            _hoverAction = hoverAction;

            _tick = 0;
            _showCursor = false;
            
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
                    if (e.Keyboard.KeyEnum.ToString().Length == 1)
                    {
                        if (e.Keyboard.KeyboardState.IsKeyDown(Keys.LeftShift) ||
                            e.Keyboard.KeyboardState.IsKeyDown(Keys.RightShift)) //if shift is held down
                            text += e.Keyboard.KeyEnum.ToString()
                                .ToUpper(); //convert the Key enum member to uppercase string
                        else
                            text += e.Keyboard.KeyEnum.ToString()
                                .ToLower(); //convert the Key enum member to lowercase string
                    }
                    else
                    {
                        switch (e.Keyboard.KeyEnum)
                        {
                            case Keys.NumPad0:
                            case Keys.D0:
                                text += "0";
                                break;
                            case Keys.NumPad1:
                            case Keys.D1:
                                text += "1";
                                break;
                            case Keys.NumPad2:
                            case Keys.D2:
                                text += "2";
                                break;
                            case Keys.NumPad3:
                            case Keys.D3:
                                text += "3";
                                break;
                            case Keys.NumPad4:
                            case Keys.D4:
                                text += "4";
                                break;
                            case Keys.NumPad5:
                            case Keys.D5:
                                text += "5";
                                break;
                            case Keys.NumPad6:
                            case Keys.D6:
                                text += "6";
                                break;
                            case Keys.NumPad7:
                            case Keys.D7:
                                text += "7";
                                break;
                            case Keys.NumPad8:
                            case Keys.D8:
                                text += "8";
                                break;
                            case Keys.NumPad9:
                            case Keys.D9:
                                text += "9";
                                break;
                            case Keys.OemPeriod:
                                text += ".";
                                break;
                            case Keys.OemSemicolon:
                                text += ":";
                                break;
                            default:
                                Console.WriteLine(e.Keyboard.KeyEnum);
                                break;
                        }
                    }
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

            _tick++;
            if (Focused && _tick % 30 == 0)
                _showCursor = !_showCursor;

            if (Focused)
            {
                if (_showCursor)
                    text += "  ";
                else
                    text += "_";
            }
                
            
            //Stop global buffer
            graphics.End();
            
            //indicate how we are redrawing the text
            RasterizerState r = new RasterizerState {ScissorTestEnable = true};
            graphics.GraphicsDevice.ScissorRectangle = new Rectangle(RenderAttributes.X,RenderAttributes.Y,RenderAttributes.Width,RenderAttributes.Height);
            
            //Start new special buffer
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,r);
            
            //Draw our text
            DrawString(graphics, font,text,new Rectangle(RenderAttributes.X, RenderAttributes.Y,RenderAttributes.Width,RenderAttributes.Height));
//            graphics.DrawString(font, text, new Vector2(RenderAttributes.X + width/2 ,RenderAttributes.Y + height/2) , RenderAttributes.Color,0,new Vector2(RenderAttributes.Width/2,RenderAttributes.Height/2), RenderAttributes.FontSize,SpriteEffects.None,1);

            //Begin next drawing after ending the special buffer
            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
//            DrawString(graphics, font, text, new Rectangle(RenderAttributes.X,RenderAttributes.Y,RenderAttributes.Width ,RenderAttributes.Height ));
        }
        
        #endregion
        
        //TODO make into helper
        private void DrawString(SpriteBatch graphics, SpriteFont font, string strToDraw, Rectangle boundaries)
        {
            
            (float x, float y) = font.MeasureString(strToDraw);

            // Taking the smaller scaling value will result in the text always fitting in the boundaries.
            float scale = boundaries.Height / y;

            int hPadding = 10;
            Vector2 position = new Vector2()//Centers the sprite
            {
                X = (x*scale + hPadding < boundaries.Width) ? boundaries.X + hPadding : boundaries.X - ((x*scale - boundaries.Width) + hPadding),
                Y = boundaries.Y - (int)(y * scale / 2) + boundaries.Height / 2 + (y*scale/7)//Centering
            };

            // Draw the string to the sprite batch!
            graphics.DrawString(font, strToDraw, position, RenderAttributes.Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
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