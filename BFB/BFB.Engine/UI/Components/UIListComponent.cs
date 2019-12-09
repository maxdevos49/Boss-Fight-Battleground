using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIListComponent<TModel, TItem> : UIComponent
    {
        private readonly Action<UIComponent, TItem> _itemTemplate;
        private readonly List<TItem> _list;
        private int _count;
        
        public UIListComponent(
            TModel model,
            Expression<Func<TModel,List<TItem>>> listSelector,
            Action<UIComponent,TItem> itemTemplate,
            StackDirection stackDirection = StackDirection.Vertical) : base(nameof(UIListComponent<TModel,TItem>),true)
        {
            _itemTemplate = itemTemplate;
            _list = listSelector.Compile().Invoke(model);
            DefaultAttributes.StackDirection = stackDirection;
            _count = 0;
        }

        #region BuildList

        private void BuildList()
        {
            Children.Clear();

            foreach (TItem listItem in _list)
                _itemTemplate(this, listItem);
            
            UIManager.BuildComponent(ParentLayer, this);
        }
        
        #endregion
        
        #region Update
        
        public override void Update(GameTime time)
        {
            if (_list.Count == _count)
                return;
            
            BuildList();
            _count = _list.Count;
        }
        
        #endregion

    }
}