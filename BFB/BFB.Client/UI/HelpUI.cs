using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;

namespace BFB.Client.UI
{
    public class HelpUI : UILayer
    {
        public HelpUI() : base(nameof(HelpUI)) { }

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
                                    h3.Text("Help");
                                })
                                .Height(0.7f)
                                .Width(0.7f)
                                .Center();
                        })
                        .Grow(3);

                    v1.Hstack(h2 =>
                    {
                        h2.Text("This is where we can explain how to play or other useful stuff that is similar. The text will scale to fit and we could also load the description from a json file or another source"); 
                        
                    }).Grow(3);
                    
                   
                    v1.Hstack(h2 =>
                    {
                        h2.Hstack(h3 =>
                            {
                                h3.Button("Back",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.Start(nameof(MainMenuUI));
                                    });
                            })
                                .Height(0.8f)
                                .Width(0.8f)
                                .Image("button")
                                .Center();
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