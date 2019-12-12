

using System;
using System.Linq.Expressions;
using BFB.Engine.Collisions;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI.Components
{
    public class UISliderComponent<TModel> : UIComponent
    {
        private const int Padding = 10;
        private const int SliderHeight = 3;
        
        private readonly TModel _model;
        private readonly Expression<Func<TModel,float>> _valueSelector;

        private readonly float _lowerRange;
        private readonly float _upperRange;

        private bool _selected;
        
        public UISliderComponent(TModel model,Expression<Func<TModel,float>> valueSelector,int lowerRange, int upperRange) : base(nameof(UISliderComponent<TModel>))
        {
            
            _model = model;
            _valueSelector = valueSelector;
            _lowerRange = lowerRange;
            _upperRange = upperRange;

            _selected = false;

            this.Color(Color.Black)
                .Background(new Color(169, 170, 168))
                .Border(3, new Color(211, 212, 210));
            
            AddEvent("click", HandleMouseClick);
            AddEvent("hover", HandleMouseHover);
            AddEvent("leave", HandleMouseLeave);
        }
        
        #region HandleMouseClick

        private void HandleMouseClick(UIEvent e)
        {
            (int x, int y, int width, int height) = ThumbBounds();
            if(Collision.IsPointColliding(e.Mouse.X, e.Mouse.Y, new System.Drawing.Rectangle(x, y, width, height)) && e.Mouse.LeftButton == ButtonState.Pressed)
                _selected = true;
        }
        
        #endregion
        
        #region HandleMouseHover

        private void HandleMouseHover(UIEvent e)
        {
            if (e.Mouse.LeftButton == ButtonState.Pressed && _selected)
            {
                int sliderLength = RenderAttributes.Width - Padding*2;
                float renderedDistance = e.Mouse.X - RenderAttributes.X;
                float percent = renderedDistance / sliderLength;
                float newValue = (_upperRange - _lowerRange) * percent;

                if (newValue > _upperRange)
                    newValue = _upperRange;

                if (newValue < _lowerRange)
                    newValue = _lowerRange;
                
                _model.SetPropertyValue(_valueSelector, newValue);
            }
            else
            {
                _selected = false;
            }
        }
        
        #endregion
        
        #region HandleMouseLeave

        private void HandleMouseLeave(UIEvent e)
        {
            _selected = false;
        }
        
        #endregion
        
        #region ThumbBounds

        private Rectangle ThumbBounds()
        {
            int sliderLength = RenderAttributes.Width - Padding*2;
            float percent =_valueSelector.Compile()(_model)/ (_upperRange - _lowerRange);
            int width = RenderAttributes.Width/15;
            int height = RenderAttributes.Height- Padding*2;
            int x = RenderAttributes.X + (int)(sliderLength * percent) - width/2;
            int y = RenderAttributes.Y + RenderAttributes.Height / 2 - SliderHeight/2 - height/2;
            
            return new Rectangle(x,y,width,height);
        }
        
        #endregion
        
        #region Render

        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            base.Render(graphics, content);

            int x = RenderAttributes.X + Padding;
            int y = RenderAttributes.Y + RenderAttributes.Height / 2 - SliderHeight/2;
            int width = RenderAttributes.Width - Padding*2;

            //Draw Rail
            graphics.Draw(content.GetTexture("default"), new Rectangle(x,y,width,SliderHeight), Color.Black);
            
            //Draw Thumb
            graphics.Draw(content.GetTexture("default"), ThumbBounds(), new Color(169, 170, 168));
            
            graphics.DrawBorder(ThumbBounds(), 3, new Color(211, 212, 210),content.GetTexture("default"));
        }

        #endregion
    }
}