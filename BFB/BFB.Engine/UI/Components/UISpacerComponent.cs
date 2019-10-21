namespace BFB.Engine.UI.Components
{
    public class UISpacerComponent:UIComponent
    {
        public UISpacerComponent(int? grow) : base(nameof(UISpacerComponent))
        {
            DefaultAttributes.Grow = grow ?? 1;
        }
    }
}