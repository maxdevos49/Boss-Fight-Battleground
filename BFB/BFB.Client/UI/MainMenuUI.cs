using System;
using BFB.Client.Scenes;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{

    public class MainMenuUI : UILayer
    {

        public MainMenuModel Model { get; set; }


        public MainMenuUI() : base(nameof(MainMenuUI))
        {
            Model = new MainMenuModel
            {
                Ip = "127.0.0.1:6969"
            };
        }

        public override void Init()
        {
            Model = new MainMenuModel
            {
                Ip = "127.0.0.1:6969"
            };
            Console.WriteLine("We have init");
            base.Init();
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
                            h2.Text("Boss Fight Battlegrounds");
                        })
                            .Grow(2);
                        
                        //Play
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Play", 
                                    clickAction: (e, a) =>
                                    {
                                        SceneManager.StartScene(nameof(PlayerConnectionScene));
                                    })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Image("button");

                            h2.TextBoxFor(Model, x => x.Ip)
                                .Width(0.8f)
                                .Height(0.8f)
                                .Grow(3)
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

                        v1.Hstack(h2 =>
                        {
                            h2.Button("Store",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.Start(nameof(StoreUI));
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

    public class MainMenuModel
    {
        public string Ip { get; set; }
    }
}