using System;
using BFB.Client.Scenes;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class LoadingGameUI : UILayer
    {
        private string LoadStatus { get; set; }

        public LoadingGameUI() : base(nameof(LoadingGameUI))
        {
            LoadStatus = "Connecting...";
        }

        protected override void Init()
        {
            LoadStatus = "Connecting...";
            AddGlobalListener("onConnectionStatus", e =>
            {
                LoadStatus = e.Message;
            });
        }
        public override void Body()
        {

            RootUI.Hstack(h1 =>
            {
                h1.Spacer();
                
                h1.Vstack(v1 =>
                {
                    v1.Spacer();
                    v1.Spacer();
                    v1.Spacer();
                    
                    v1.TextFor(this, x => x.LoadStatus)
                        .Grow(2);
                    
                    v1.Spacer();
                   
                    v1.Button("Cancel", clickAction: (e, a) =>
                    {
                        SceneManager.StartScene(nameof(MainMenuScene));
                    })
                        .Image("button");
                    
                    v1.Spacer();
                    v1.Spacer();

                })
                    .Grow(2);
                
                h1.Spacer();
            }).Background(Color.Brown);

        }
    }
    
}