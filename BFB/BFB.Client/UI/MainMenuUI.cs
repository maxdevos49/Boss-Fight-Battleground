using System;
using BFB.Client.Scenes;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.UI
{

    public class MainMenuUI : UILayer
    {

        private string Ip { get; set; }

        public MainMenuUI() : base(nameof(MainMenuUI))
        {
        }

        protected override void Init()
        {
            Ip = ServerMenuModel.GetServers().DirectConnect;
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
                                h2.Text("Boss Fight Battlegrounds")
                                    .Color(Color.White)
                                    .FontSize(1.5f);
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
                                            ClientDataRegistry.Ip = Ip.Split(":")[0];
                                            ClientDataRegistry.Port = Convert.ToInt32(Ip.Split(":")[1]);
                                            SceneManager.StartScene(nameof(GameScene));
                                        });

                                h3.TextBoxFor(this, x => x.Ip)
                                    .Background(Color.White)
                                    .Grow(3)
                                    .Color(Color.Black);
                                })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                        //Server Menu
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Server Menu",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.StartLayer(nameof(ServerMenuUI),ParentScene);
                                    })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                        //Settings
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Settings",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(SettingsUI), ParentScene); })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                        //Help
                        v1.Hstack(h2 =>
                        {
                            h2.Button("Help",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(HelpUI), ParentScene); })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                        v1.Hstack(h2 =>
                        {
                            h2.Button("Store",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(StoreUI), ParentScene); })
                                .Width(0.8f)
                                .Height(0.8f)
                                .Center();
                        });

                        v1.Spacer();
                    })
                    .Grow(3)
                    .Center();

                h1.Spacer();
            });
        }
    }
}