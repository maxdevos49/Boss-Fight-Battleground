using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class HudUI : UILayer
    {
        public HudUI() : base(nameof(HudUI))
        {
            BlockInput = false;//Prevents moving our player from inside the UI
        }

        protected override void Init()
        {
            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                        UIManager.StartLayer(nameof(GameMenuUI),ParentScene);
                        break;
                    case Keys.E:
                        UIManager.LaunchLayer(nameof(InventoryUI), ParentScene);
                        break;
                    case Keys.T:
                        UIManager.LaunchLayer(nameof(ChatUI), ParentScene);
                        break;
                }
            });
            
            
//            UIManager.LaunchLayer(nameof(ChatUI), ParentScene);
        }

        public override void Body()
        {
            //Change this to draw frame outlines or not

            RootUI.Background(Color.Transparent);
            
            RootUI.Zstack(z1 => { 
                z1.Vstack(v1 =>
                    {
                        v1.Button("Menu",
                                clickAction: (e, a) => { UIManager.StartLayer(nameof(GameMenuUI), ParentScene); })
                            .Width(0.12f)
                            .Height(0.1f);
                    })
                    .Top(0)
                    .Left(0);

                z1.Hstack(h1 =>
                    {
                        h1.Spacer(2);

                        h1.Hstack(h2 =>
                            {
                

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