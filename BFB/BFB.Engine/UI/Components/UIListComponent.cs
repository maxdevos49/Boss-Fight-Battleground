using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BFB.Engine.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIListComponent<TModel, TItem> : UIComponent
    {
        private readonly Action<UIComponent, TItem> _itemTemplate;
        private readonly List<TItem> _list;
        private readonly UIComponent _stack;
        
        private int _scrollPosition;
        private int _targetScrollPosition;
        private int _count;
        
        public UIListComponent(
            TModel model,
            Expression<Func<TModel,List<TItem>>> listSelector,
            Action<UIComponent,TItem> itemTemplate,
            StackDirection stackDirection = StackDirection.Vertical) : base(nameof(UIListComponent<TModel,TItem>),true)
        {
            Focusable = true;
            _itemTemplate = itemTemplate;
            _list = listSelector.Compile().Invoke(model);
            _stack = stackDirection == StackDirection.Horizontal ? this.Hstack(x => { }) : this.Vstack(x => { });
            
            DefaultAttributes.Overflow = Overflow.Hide;
            _stack.DefaultAttributes.Sizing = Sizing.Dimension;
            
            _scrollPosition = 0;
            _targetScrollPosition = 0;
            _count = 0;
            
            AddEvent("mousescroll", HandleMouseScroll);
        }

        #region HandleMouseScroll
        
        private void HandleMouseScroll(UIEvent e)
        {
            const int fraction = 10;
            
            _targetScrollPosition = _scrollPosition + e.Mouse.ScrollAmount/fraction;

            int scrollBottom;
            
            if (_stack.DefaultAttributes.StackDirection == StackDirection.Horizontal)
            {
                int height = _stack.Children.Sum(x => x.RenderAttributes.Height);
                scrollBottom = RenderAttributes.Height - height;
            }
            else
            {
                int width = _stack.Children.Sum(x => x.RenderAttributes.Width);//TODO broken. Cuts off numbers
                scrollBottom = RenderAttributes.Width - width;
            }
            
            
            if (_targetScrollPosition < scrollBottom - fraction)
                _targetScrollPosition = scrollBottom - fraction;

            if (_targetScrollPosition > fraction)
                _targetScrollPosition = fraction;
        }
        
        #endregion

        #region BuildList

        private void BuildList()
        {
            _stack.Children.Clear();

            foreach (TItem listItem in _list)
                _itemTemplate(_stack, listItem);
            
            UIManager.BuildComponent(ParentLayer, this);
        }
        
        #endregion
        
        #region Update
        
        public override void Update(GameTime time)
        {
            if (_list.Count != _count)
            {
                BuildList();
                _count = _list.Count;
                return;
            }
            
            if (_scrollPosition != _targetScrollPosition)
            {
                if (_targetScrollPosition > _scrollPosition)
                    _scrollPosition += System.Math.Abs(_targetScrollPosition - _scrollPosition)/10;
                else
                    _scrollPosition -= System.Math.Abs(_targetScrollPosition - _scrollPosition)/10;

                if (System.Math.Abs(_targetScrollPosition - _scrollPosition) < 2)
                    _scrollPosition = _targetScrollPosition;
            }

            if (_stack.DefaultAttributes.StackDirection == StackDirection.Horizontal)
                _stack.RenderAttributes.OffsetX = _scrollPosition;
            else
                _stack.RenderAttributes.OffsetY = _scrollPosition;
            
        }
        
        #endregion

    }
}