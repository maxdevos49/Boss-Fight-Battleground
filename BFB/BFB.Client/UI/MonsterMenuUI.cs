using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class MonsterMenuUI : UILayer
    {
        public MonsterMenuUI() : base(nameof(MonsterMenuUI)) { }

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
                        h2.Hstack(h3 => { h3.Text("Select Your Monster"); })
                            .Height(0.7f)
                            .Width(0.7f)
                            .Center();
                    })
                        .Grow(3);

                    v1.Hstack(h2 =>
                    {
                        h2.Button("Spider",
                                clickAction: (e, a) => { UIManager.Start(nameof(HudUI)); })
                            .Height(0.8f)
                            .Width(0.8f)
                            .Image("button")
                            .Center();
                    });

                    v1.Hstack(h2 =>
                    {
                        h2.Button("Skeleton",
                                clickAction: (e, a) => { UIManager.Start(nameof(HudUI)); })
                            .Height(0.8f)
                            .Width(0.8f)
                            .Image("button")
                            .Center();
                    });

                    v1.Hstack(h2 =>
                    {
                        h2.Button("Zombie",
                                clickAction: (e, a) => { UIManager.Start(nameof(HudUI)); })
                            .Height(0.8f)
                            .Width(0.8f)
                            .Image("button")
                            .Center();
                    });

                    v1.Hstack(h2 =>
                    {
                        h2.Button("THE BOSS",
                                clickAction: (e, a) => { UIManager.Start(nameof(HudUI)); })
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