using System;
using System.Collections.Generic;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Client.UI
{
    public class ChatUI : UILayer
    {
        public ChatModel Model { get; set; }
        
        public ChatUI() : base(nameof(ChatUI))
        {
            Model = new ChatModel
            {
                TextBoxText = ""
            };
        }
        
        public override void Body()
        {
            Debug = false;
            
            RootUI.Zstack(z1 =>
            {
                z1.Vstack(v1 =>
                {
                    v1.Spacer(3);
                    
                    v1.Hstack(h1 =>
                    {
                        h1.Spacer(3);
                        
                        h1.Vstack(v2 =>
                        {
                            v2.Text("Test message").Background(new Color(0, 0, 0, 100));
                            v2.Text("Test message").Background(new Color(0, 0, 0, 100));
                            v2.Text("Test message").Background(new Color(0, 0, 0, 100));
                            v2.Text("Test message").Background(new Color(0, 0, 0, 100));
                            v2.Text("Test message").Background(new Color(0, 0, 0, 100));
                            v2.Text("Test message").Background(new Color(0, 0, 0, 100));
                            v2.TextBoxFor(Model, t => t.TextBoxText).Background(new Color(0, 0, 0, 100));
                        }).Grow(2);
                    }).Grow(2);
                });
            });
        }
    }

    public class ChatModel
    {
        public string TextBoxText { get; set; }

        public const int NumMessages = 8;

        public List<string> Messages;
    }
}