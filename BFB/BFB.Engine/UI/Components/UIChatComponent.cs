using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using BFB.Engine.Helpers;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI
{
    public class UIChatComponent : UIComponent
    {
        
        public List<ChatMessage> Messages { get; set; }
        
        public UIChatComponent() : base(nameof(UIChatComponent))
        {
            this.Overflow(Overflow.Hide);

            ChatMessage message = new ChatMessage
            {
                Header = new ChatText("[Madmax]"),
                Body = new List<ChatText>
                {
                    new ChatText("This is a chat messages message")
                }
            };
        }


        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            
            
            
        }
    }

    [Serializable]
    public class ChatText
    {
        [UsedImplicitly]
        public BfbColor Color { get; set; }
        
        [UsedImplicitly]
        public string Text { get; set; }

        public ChatText(string text, BfbColor color = null)
        {
            Text = text;
            Color = color ?? new BfbColor(0,0,0,1);
        }
    }

    [Serializable]
    public class ChatMessage
    {
        public ChatText Header { get; set; }
        
        public List<ChatText> Body { get; set; }

    }
}