using BFB.Client.Helpers;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class AddServerUI : UILayer
    {
        private ServerMenuItem NewServer { get; set; }

        public AddServerUI() : base(nameof(AddServerUI))  {}

        protected override void Init()
        {
            NewServer = new ServerMenuItem();
        }

        public override void Body()
        {
            RootUI.Vstack(v1 =>
                {
                    v1.Text("Add Server").FontSize(1.6f).Color(Color.White);

                    v1.Spacer(2);

                    v1.Vstack(v2 =>
                    {
                            v2.Text("Server name:").JustifyText(JustifyText.Start).Color(Color.White);
                            v2.TextBoxFor(NewServer, x => x.Name)
                                .Background(Color.White)
                                .Color(Color.Black);

                            v2.Text("IP Address:").JustifyText(JustifyText.Start).Color(Color.White);
                            v2.TextBoxFor(NewServer, x => x.Ip)
                                .Background(Color.White)
                                .Color(Color.Black);
                    }).Grow(13);
                    
                    v1.Spacer(6);

                    //Buttons
                    v1.Hstack(h2 =>
                    {
                        //Save Server
                        h2.Vstack(v3 =>
                        {
                            v3.Button("Save Server",
                                    clickAction: (e, a) =>
                                    {
                                        if (string.IsNullOrEmpty(NewServer.Name) || string.IsNullOrEmpty(NewServer.Ip))
                                            return;
                                        
                                        ClientSettings existingServers = ClientSettings.GetSettings();
                                        existingServers.ServerSettings.Servers.Add(NewServer);
                                        ClientSettings.SaveSettings(existingServers);
                                        UIManager.StartLayer(nameof(ServerMenuUI), ParentScene);
                                    })
                                .Height(0.8f)
                                .Center();
                        }).Grow(8);

                        h2.Spacer();

                        //Cancel
                        h2.Vstack(v3 =>
                        {
                            v3.Button("Cancel",
                                    clickAction: (e, a) =>
                                    {
                                        UIManager.StartLayer(nameof(ServerMenuUI), ParentScene);
                                    })
                                .Height(0.8f)
                                .Center();
                        }).Grow(8);
                    }).Grow(3);
                })
                .Width(0.8f)
                .Height(0.8f)
                .Grow(2)
                .Center();
        }
    }
}