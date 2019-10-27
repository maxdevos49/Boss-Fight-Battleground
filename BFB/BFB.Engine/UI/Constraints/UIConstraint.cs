using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Constraints
{
    public abstract class UIConstraint
    {

        public string Name { get; set; }

        public UIConstraint(string name)
        {
            Name = name;
        }
        
        public abstract void Apply(UIComponent component);

    }
}