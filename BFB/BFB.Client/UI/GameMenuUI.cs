using BFB.Client.Scenes;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class GameMenuUI : UILayer
    {
        public GameMenuUI() : base(nameof(GameMenuUI))
        {
            BlockInput = true;
        }

        protected override void Init()
        {
            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                        UIManager.StartLayer(nameof(HudUI), ParentScene);
                        break;
                }
            });
        }

        public override void Body()
        {
            RootUI.Hstack(h1 =>
                {

                    h1.Spacer();

                    h1.Vstack(v1 =>
                        {

                            v1.Spacer();

                            v1.Hstack(h2 =>
                                {
                                    h2.Hstack(h3 => { h3.Text("Menu"); })
                                        .Height(0.7f)
                                        .Width(0.7f)
                                        .Center();
                                })
                                .Grow(3);

                            v1.Hstack(h2 =>
                            {
                                h2.Button("Main Menu",
                                        clickAction: (e, a) => { SceneManager.StartScene(nameof(MainMenuScene)); })
                                    .Height(0.8f)
                                    .Width(0.8f)
                                    .Image("button")
                                    .Center();
                            });

                            v1.Hstack(h2 =>
                            {
                                h2.Button("Back to Game",
                                        clickAction: (e, a) => { UIManager.StartLayer(nameof(HudUI), ParentScene); })
                                    .Height(0.8f)
                                    .Width(0.8f)
                                    .Image("button")
                                    .Center();
                            });

                            v1.Spacer(3);

                        })
                        .Grow(3);

                    h1.Spacer();

                })
                .Background(new Color(0, 0, 0, 100));
        }
    }

    
}