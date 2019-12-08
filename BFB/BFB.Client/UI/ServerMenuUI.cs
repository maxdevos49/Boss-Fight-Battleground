using System;
using System.Collections.Generic;
using System.IO;
using BFB.Client.Scenes;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
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
            Model = ServerMenuModel.GetServers();
        }

        public override void Body()
        {
            Color bgColor = new Color(130, 131, 129);
            bool bg = false;

            RootUI.Vstack(v1 =>
                {
                    v1.Text("Server Menu").FontSize(1.6f).Color(Color.White);

                    v1.Spacer();

                    v1.Hstack(h2 =>
                        {
                            h2.ScrollableContainer(s3 =>
                            {
                                s3.ListFor(Model, x => x.Servers, (stack4, item) =>
                                {
                                    stack4.Hstack(h5 =>
                                        {
                                            h5.Spacer();

                                            h5.Vstack(v6 =>
                                                {
                                                    v6.Text(item.Name).JustifyText(JustifyText.Start);
                                                    v6.Text(item.Ip).JustifyText(JustifyText.Start).FontSize(0.75f);
                                                })
                                                .Grow(40);

                                            h5.Vstack(v6 =>
                                            {
                                                v6.Spacer();

                                                v6.Button("Join",
                                                        clickAction: (e, a) =>
                                                        {
                                                            ClientDataRegistry.Ip = item.Ip.Split(":")[0];
                                                            ClientDataRegistry.Port =
                                                                Convert.ToInt32(item.Ip.Split(":")[1]);
                                                            SceneManager.StartScene(nameof(GameScene));
                                                        })
                                                    .Center()
                                                    .Grow(18);

                                                v6.Spacer();
                                            }).Grow(5);

                                            h5.Spacer();
                                        })
                                        .Background(bgColor)
                                        .Height(0.23f);

                                    if (bg)
                                    {
                                        bg = false;
                                        bgColor = new Color(130, 131, 129);
                                    }
                                    else
                                    {
                                        bg = true;
                                        bgColor = new Color(169, 170, 168);
                                    }
                                });
                            });
                        })
                        .Grow(15);

                    v1.Hstack(h2 =>
                    {
                        //Add Server
                        h2.Vstack(v3 =>
                        {
                            v3.Button("Add Server",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(AddServerUI), ParentScene); })
                                .Height(0.8f)
                                .Center();
                        }).Grow(8);

                        h2.Spacer();

                        //Edit Servers
                        h2.Vstack(v3 =>
                        {
                            v3.Button("Edit Servers",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.StartLayer(nameof(EditServerListUI), ParentScene);
                                    })
                                .Height(0.8f)
                                .Center();
                        }).Grow(8);
                    }).Grow(3);

                    //Back
                    v1.Button("Back",
                        clickAction: (e, a) => { UIManager.StartLayer(nameof(MainMenuUI), ParentScene); }).Grow(2);
                })
                .Width(0.8f)
                .Height(0.8f)
                .Grow(2)
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
        public List<ServerMenuItem> Servers { get; set; }

        public string DirectConnect { get; set; }

        public ServerMenuModel()
        {
            Servers = new List<ServerMenuItem>();
        }

        public static ServerMenuModel GetServers()
        {
            string json;

            //Get file for Parsing
            using (StreamReader r = new StreamReader("server.json"))
            {
                json = r.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ServerMenuModel>(json);
        }

        public static void SaveServer(ServerMenuModel model)
        {
            string serializedWorldData = JsonConvert.SerializeObject(model, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            try
            {
                string path = Directory.GetCurrentDirectory() + "/server.json";

                using (StreamWriter s = File.CreateText(path))
                {
                    s.Write(serializedWorldData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}