using System.Collections.Generic;

namespace BFB.Engine.UI.Components
{
    public interface INode
    {
        /**
         * Refers to the node that owns this components
         */
        IComponent Parent { get; set; }
        
        /**
         * Contains all of the children of this Node
         */
        List<IComponent> Children { get; set; }

        /**
         * Adds a new child node
         */
        void AddChild(IComponent component);
    }
}