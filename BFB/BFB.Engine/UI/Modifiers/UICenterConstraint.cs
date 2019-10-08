using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Modifiers
{
    public class UICenterConstraint : UIConstraint
    {

        public UICenterConstraint() : base(nameof(UICenterConstraint)) { }

        public override void Apply(UIComponent component)
        {
            component.X = component.Parent.X + ((component.Parent.Width - component.Width) / 2);
            component.Y = component.Parent.Y + ((component.Parent.Height - component.Height) / 2);
        }
    }
}