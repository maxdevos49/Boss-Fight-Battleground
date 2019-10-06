using System;
using BFB.Engine.UI.Components;

namespace BFB.Engine.UI
{
    // ReSharper disable once InconsistentNaming
    public interface UIView
    {
        IComponent Body<TModel>(UIContext<TModel> ui);
    }
}