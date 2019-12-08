using System;
using System.Collections.Generic;
using System.IO;
using BFB.Client.Scenes;
using BFB.Engine.Content;
using BFB.Engine.UI;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace BFB.Client.UI
{
    public class ServerMenuUI : UILayer
    {
        private ServerMenuModel Model { get; set; }

        public ServerMenuUI() : base(nameof(ServerMenuUI))
        {
        }

        protected override void Init()
        {
            string json;

            //Get file for Parsing
            using (StreamReader r = new StreamReader("server.json"))
            {
                json = r.ReadToEnd();
            }

            Model = JsonConvert.DeserializeObject<ServerMenuModel>(json);
        }

        public override void Body()
        {
            RootUI.Vstack(v1 =>
                {
                    v1.Hstack(h1 => { h1.Text("Server Menu").FontSize(1.4f); });

                    v1.Hstack(h2 =>
                    {
                        h2.ScrollableContainer(s1 =>
                            {
                                s1.ListFor(Model, x => x.Servers, (stack, item) =>
                                {
                                    stack.Hstack(h3 =>
                                        {
                                            h3.Vstack(v2 =>
                                                {
                                                    v2.Text(item.Name).JustifyText(JustifyText.Start);
                                                    v2.Text(item.Ip).JustifyText(JustifyText.Start).FontSize(0.75f);
                                                })
                                                .Grow(7);

                                            h3.Button("Join",
                                                    clickAction: (e, a) =>
                                                    {
                                                        MainMenuUI m =
                                                            (MainMenuUI) UIManager.GetLayer(nameof(MainMenuUI));
                                                        m.Model.Ip = item.Ip;
                                                        SceneManager.StartScene(nameof(PlayerConnectionScene));
                                                    })
                                                .Image("button")
                                                .Height(0.9f);
                                        })
                                        .Height(0.2f);
                                });
                            })
                            .Border(3);
                    }).Grow(7);

                    v1.Hstack(h10 =>
                    {
                        h10.Vstack(v10 =>
                        {
                            v10.Button("Add Server",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI), ParentScene); })
                                .Height(0.9f)
                                .Center()
                                .Image("button");
                        }).Grow(8);

                        h10.Spacer();

                        h10.Vstack(v11 =>
                        {
                            v11.Button("Delete Server",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI), ParentScene); })
                                .Height(0.9f)
                                .Center()
                                .Image("button");
                        }).Grow(8);
                    });

                    v1.Button("Back",
                            clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI), ParentScene); })
                        .Image("button");
                })
                .Width(0.9f)
                .Height(0.9f)
                .Center();
        }
    }

    public class ServerMenuItem
    {
        public string Name { get; set; }

        public string Ip { get; set; }
    }

    public class ServerMenuModel
    {
        public ServerMenuModel()
        {
            Servers = new List<ServerMenuItem>();
        }

        public List<ServerMenuItem> Servers { get; set; }

        public string DirectConnect { get; set; }
    }
}