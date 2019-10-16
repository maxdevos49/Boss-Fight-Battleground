
using BFB.Engine.Event;
using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIButtonComponent : UIComponent
    {

        public UIButtonComponent(string text) : base(nameof(UIButtonComponent))
        {
            //add text
            this.Text(text)
                .Color(Color)
                .Background(Background);
        }
    }
}