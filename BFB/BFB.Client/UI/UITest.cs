using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{

    public class UITest : UILayer
    {
        public UITest() :base(nameof(UITest)) { }
        
        public override void Body()
        {
            RootUI.Vstack((v1) =>
            {
                v1.Vstack((v2) =>
                {
                    v2.Vstack((v3) =>
                    {
                        
                    })
                        .Width(0.8f)
                        .Height(0.5f)
                        .Background(Color.Yellow);
                    
                })
                    .Width(0.8f)
                    .Height(0.5f)
                    .Background(Color.Green);
                
            })
                .Width(0.5f)
                .Height(0.5f)
                .Background(Color.Red);
            /**
             * Evaluate the entire structure and then generate its
             * positions. That way we know number of elements
             * everywhere and we can then have all constraints
             * stored for full processing and we dont get have
             * issues embedding elements inside of other elements
             */

        }
    }
}