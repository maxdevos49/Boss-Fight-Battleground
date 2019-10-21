using System;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{

    public class MainMenuUI : UILayer
    {

        private TestModel model;

        public MainMenuUI() : base(nameof(MainMenuUI))
        {
            model = new TestModel
            {
                Test1 = "Boss Fight Battlegrounds"
            };
        }
        
        
        
        public override void Body()
        {
            RootUI.Hstack(h1 =>
                {
                h1.Spacer();

                h1.Vstack(v1 =>
                    {
                        v1.Spacer();
                        
                        //Game title
                        v1.Hstack(h2 =>
                        {
                            h2.TextFor(model,x => x.Test1);

                        })
                            .Grow(2);
                        
                        //Play
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Play", hoverAction: (e, a) =>
                                {
                                    a.Background = Color.Blue;
                                }, clickAction: (e, a) =>
                                {
                                    a.Background = Color.Green;
                                })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Image("button");

                            h2.TextBoxFor(model, x => x.Test1)
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Background(Color.White)
                                .Color(Color.Black);
                            
                        });
                        
                        //Settings
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Settings",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.Start(nameof(SettingsUI));
                                    })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Image("button");
                        });
                        
                        //Help
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Help", 
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.Start(nameof(HelpUI));
                                    })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Image("button");
                        });
                        
                        v1.Spacer();
                    })
                    .Grow(3)
                    .Center();

                    h1.Spacer();
                })
                .AspectRatio(1.78f)
                .Center();
        }
    }

    public class TestModel
    {
        public string Test1 { get; set; }
        public string Test2 { get; set; }
    }
}