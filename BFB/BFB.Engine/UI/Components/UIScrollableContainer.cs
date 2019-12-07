using System.Linq;
using BFB.Engine.Event;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIScrollableContainer : UIComponent
    {
        private UIComponent _scrollComponent;
        private int _scrollPosition;
        private int _targetScrollPosition;
        private bool _init;
        
        public UIScrollableContainer() : base(nameof(UIScrollableContainer), true)
        {
            _init = false;
            _scrollPosition = 0;
            _targetScrollPosition = 0;
            
            DefaultAttributes.Overflow = Overflow.Hide;
            AddEvent("mousescroll", HandleMouseScroll);
        }

        public void AddStack(UIComponent component)
        {
            _scrollComponent = component;
            _scrollComponent.DefaultAttributes.Sizing = Sizing.Dimension;
            _init = true;
        }
        
        #region HandleMouseScroll
        
        private void HandleMouseScroll(UIEvent e)
        {
            if(!_init)
                return;
            
            const int fraction = 10;
            
            _targetScrollPosition = _scrollPosition + e.Mouse.VerticalScrollAmount/fraction;

            int scrollBottom;
            
            if (_scrollComponent.DefaultAttributes.StackDirection == StackDirection.Horizontal)
            {
                int height = _scrollComponent.Children.Sum(x => x.RenderAttributes.Height);//TODO broken. Cuts off items
                scrollBottom = RenderAttributes.Height - height;
            }
            else
            {
                int width = _scrollComponent.Children.Sum(x => x.RenderAttributes.Width);
                scrollBottom = RenderAttributes.Width - width;
            }
            
            
            if (_targetScrollPosition < scrollBottom - fraction)
                _targetScrollPosition = scrollBottom - fraction;

            if (_targetScrollPosition > fraction)
                _targetScrollPosition = fraction;
        }
        
        #endregion

        #region Update
        
        public override void Update(GameTime time)
        {
            if(!_init)
                return;
            
            if (_scrollPosition != _targetScrollPosition)
            {
                if (_targetScrollPosition > _scrollPosition)
                    _scrollPosition += System.Math.Abs(_targetScrollPosition - _scrollPosition)/10;
                else
                    _scrollPosition -= System.Math.Abs(_targetScrollPosition - _scrollPosition)/10;

                if (System.Math.Abs(_targetScrollPosition - _scrollPosition) < 2)
                    _scrollPosition = _targetScrollPosition;
            }

            if (_scrollComponent.DefaultAttributes.StackDirection == StackDirection.Horizontal)
                _scrollComponent.RenderAttributes.OffsetX = _scrollPosition;
            else
                _scrollComponent.RenderAttributes.OffsetY = _scrollPosition;
        }
        
        #endregion
    }
}