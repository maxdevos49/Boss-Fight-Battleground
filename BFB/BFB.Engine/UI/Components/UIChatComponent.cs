using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIChatComponent : UIComponent
    {
        private List<ChatMessage> Messages { get; set; }
        
        public UIChatComponent() : base(nameof(UIChatComponent))
        {
            Messages = new List<ChatMessage>();

            this
                .Background(Color.Black)
                .FontSize(0.5f);

            
            
            ChatMessage message = new ChatMessage
            {
                Header = new ChatText("[Madmax]", ColorOption.Yellow),
                Body = new List<ChatText>
                {
                    new ChatText("This is a chat messages message.", ColorOption.Blue),
                    new ChatText("This is the second Part", ColorOption.Green)
                }
            };
            
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
            Messages.Add(message);
        }

        
        

        public override void Render(SpriteBatch graphics, BFBContentManager content)
        {
            base.Render(graphics,content);

            int height = 0;
            foreach (ChatMessage chatMessage in Messages)
               height += graphics.DrawChatText(height, chatMessage, this, content);

            RenderAttributes.Height = height;
        }
    }

    [Serializable]
    public class ChatText
    {
        public ColorOption ForegroundColor { get; set; }

        public string Text { get; }

        public ChatText(string text, ColorOption foregroundColor = ColorOption.White)
        {
            Text = text;
            ForegroundColor = foregroundColor;
        }
    }

    [Serializable]
    public class ChatMessage
    {
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public ColorOption BackgroundColor { get; set; }
        
        public ChatText Header { get; set; }
        
        public List<ChatText> Body { get; set; }
  
        
    }

    
}