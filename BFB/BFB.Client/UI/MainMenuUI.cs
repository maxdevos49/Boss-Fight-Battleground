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

        protected override void Init()
        {
//            Model = new MainMenuModel
//            {
//                Ip = "127.0.0.1:6969"
//            };
//            Console.WriteLine("We have init");
//            base.Init();
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
                            h2.Hstack(h3 =>
                            {
                                h3.Button("Play",
                                        clickAction: (e, a) =>
                                        {
                                            SceneManager.StartScene(nameof(PlayerConnectionScene));
                                        })
                                    .Image("button");

                                h3.TextBoxFor(Model, x => x.Ip)
                                    .Background(Color.White)
                                    .Grow(3)
                                    .Color(Color.Black);
                                
                                h3.Button("Server Menu",
                                        clickAction: (e, a) =>
                                        {
                                            UIManager.StartLayer(nameof(ServerMenuUI),ParentScene);
                                        })
                                    .Image("button");
                            })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });
                        
                        //Settings
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Settings",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.StartLayer(nameof(SettingsUI),ParentScene);
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
                                        UIManager.StartLayer(nameof(HelpUI),ParentScene);
                                    })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center()
                                .Image("button");
                        });

                        v1.Hstack(h2 =>
                        {
                            h2.Button("Store",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(StoreUI), ParentScene); })
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