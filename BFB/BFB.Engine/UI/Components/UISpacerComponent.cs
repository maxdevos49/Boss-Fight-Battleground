namespace BFB.Engine.UI.Components
{
    public class UISpacerComponent:UIComponent
    {
        public UISpacerComponent(int? grow) : base(nameof(UISpacerComponent))
        {
            Grow = grow ?? 1;
            StackDirection = StackDirection.None;
        }
    }
}