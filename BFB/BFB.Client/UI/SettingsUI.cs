using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;

namespace BFB.Client.UI
{
    public class SettingsUI : UILayer
    {
        public SettingsUI() : base(nameof(SettingsUI)) { }

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
                                h2.Hstack(h3 =>
                                    {
                                        h3.Text("Settings");
                                    })
                                    .Height(0.7f)
                                    .Width(0.7f)
                                    .Center();
                            })
                            .Grow(3);

                        v1.Hstack(h2 =>
                        {
                            h2.Button("Connection Options",
                                    clickAction: (e, a) =>
                                    {
                                        
                                    })
                                .Height(0.8f)
                                .Width(0.8f)
                                .Image("button")
                                .Center();
                        });
                        
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Controls",
                                    clickAction: (e, a) =>
                                    {
                                        
                                    })
                                .Height(0.8f)
                                .Width(0.8f)
                                .Image("button")
                                .Center();
                        });
                        
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Video Options",
                                    clickAction: (e, a) =>
                                    {
                                        
                                    })
                                .Height(0.8f)
                                .Width(0.8f)
                                .Image("button")
                                .Center();
                        });
                        
                        v1.Hstack(h2 =>
                        {
                            h2.Hstack(h3 =>
                            {
                                h3.Button("Back",
                                    clickAction: (e, a) =>
                                    {
<<<<<<< HEAD
                                        UIManager.StartLayer(nameof(MainMenuUI)); 
=======
                                        UIManager.Start(nameof(MainMenuUI),ParentScene); 
>>>>>>> 25eb9767d2ff3738c9e8f07fa547477d11c8cf6b
                                    })
                                    .Height(0.8f)
                                    .Width(0.8f)
                                    .Image("button")
                                    .Center();
                            });
                        });
                        
                        v1.Spacer();
                    })
                        .Grow(3);
                
                    h1.Spacer();

                })
                    .AspectRatio(1.78f)
                    .Center();
        }
    }
}