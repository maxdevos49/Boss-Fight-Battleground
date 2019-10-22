using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Constraints
{
    public class UICenterConstraint : UIConstraint
    {

        public UICenterConstraint() : base(nameof(UICenterConstraint)) { }

        public override void Apply(UIComponent component)
        {
            component.DefaultAttributes.X = component.Parent.DefaultAttributes.X + ((component.Parent.DefaultAttributes.Width - component.DefaultAttributes.Width) / 2);
            component.DefaultAttributes.Y = component.Parent.DefaultAttributes.Y + ((component.Parent.DefaultAttributes.Height - component.DefaultAttributes.Height) / 2);
        }
    }
}