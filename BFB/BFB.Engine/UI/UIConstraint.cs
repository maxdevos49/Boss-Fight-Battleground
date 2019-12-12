using JetBrains.Annotations;

namespace BFB.Engine.UI
{
    public abstract class UIConstraint
    {

        [UsedImplicitly]
        public string Name { get; set; }

        protected UIConstraint(string name)
        {
            Name = name;
        }
        
        public abstract void Apply(UIComponent component);

    }
}