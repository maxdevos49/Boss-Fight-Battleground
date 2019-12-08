using BFB.Engine.UI;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class EditServerListUI : UILayer
    {
        private ServerMenuModel Model { get; set; }

        public EditServerListUI() : base(nameof(EditServerListUI))
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
                    v1.Text("Edit Servers").FontSize(1.6f).Color(Color.White);

                    v1.Spacer();

                    v1.Hstack(h2 =>
                        {
                            h2.ScrollableContainer(s3 =>
                            {
                                int counter = 0;

                                s3.ListFor(Model, x => x.Servers, (stack4, item) =>
                                {
                                    var counter3 = counter;
                                    stack4.Hstack(h5 =>
                                        {
                                            h5.Spacer();

                                            h5.Vstack(v6 =>
                                                {
                                                    v6.Text(item.Name).JustifyText(JustifyText.Start);
                                                    v6.Text(item.Ip).JustifyText(JustifyText.Start).FontSize(0.75f);
                                                })
                                                .Grow(30);

                                            var counter4 = counter3;
                                            h5.Vstack(v6 =>
                                                {
                                                    v6.Spacer();

                                                    var counter1 = counter4;
                                                    v6.Button("Favorite",
                                                            clickAction: (e, a) =>
                                                            {
                                                                int index = counter1;
                                                                var existingServers = ServerMenuModel.GetServers();
                                                                existingServers.DirectConnect = existingServers.Servers[index].Ip;
                                                                ServerMenuModel.SaveServer(existingServers);
                                                            })
                                                        .Center()
                                                        .Grow(18);

                                                    v6.Spacer();
                                                })
                                                .Grow(8);

                                            h5.Spacer();

                                            var counter2 = counter3;
                                            h5.Vstack(v6 =>
                                                {
                                                    v6.Spacer();

                                                    var counter1 = counter2;
                                                    v6.Button("Delete",
                                                            clickAction: (e, a) =>
                                                            {
                                                                int index = counter1;
                                                                var existingServers = ServerMenuModel.GetServers();
                                                                existingServers.Servers.RemoveAt(index);
                                                                ServerMenuModel.SaveServer(existingServers);
                                                                UIManager.StartLayer(nameof(EditServerListUI),
                                                                    ParentScene);
                                                            })
                                                        .Center()
                                                        .Grow(18);

                                                    v6.Spacer();
                                                })
                                                .Grow(6);

                                            h5.Spacer();
                                        })
                                        .Background(bgColor)
                                        .Height(0.23f);

                                    counter++;

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

                        //Cancel
                        h2.Vstack(v3 =>
                        {
                            v3.Button("Cancel",
                                    clickAction: (e, a) => { UIManager.StartLayer(nameof(ServerMenuUI), ParentScene); })
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
}