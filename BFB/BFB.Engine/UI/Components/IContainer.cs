using System.Collections.Generic;

namespace BFB.Engine.UI.Components
{
    public interface IContainer : IComponent
    {
        Stack<IComponent> Contents { get; set; }
    }
}