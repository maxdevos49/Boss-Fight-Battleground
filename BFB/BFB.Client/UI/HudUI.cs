using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class HudUI : UILayer
    {
        public HudUI() : base(nameof(HudUI)) { }

        public override void Body()
        {
            //Change this to draw frame outlines or not
            Debug = true;
            
            RootUI.Zstack(z1 => { 
                z1.Vstack(v1 =>
                    {
                        v1.Button("Menu",
                                clickAction: (e, a) =>
                                {
                                    UIManager.Start(nameof(GameMenuUI));
                                })
                            .Width(0.12f)
                            .Height(0.1f)
                            .Image("button");
                    })
                    .Top(0)
                    .Left(0);

                z1.Hstack(h1 =>
                    {
                        h1.Spacer(2);

                        h1.Hstack(h2 =>
                            {
                                h2.Button("Slot 0")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 1")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 2")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 3")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 4")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 5")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 6")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 7")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 8")
                                    .Background(new Color(0, 0, 0, 100));
                                h2.Button("Slot 9")
                                    .Background(new Color(0, 0, 0, 100));

                            })
                            .Grow(10);
                        
                        h1.Spacer(2);
                    })
                    .Height(0.07f)
                    .Bottom(0);
            });
            
        }
    }
}