using System.Collections.Generic;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class ChatUI : UILayer
    {
        private ChatModel Model { get; set; }
        
        public ChatUI() : base(nameof(ChatUI))
        {
            BlockInput = true;
            
            
        }

        protected override void Init()
        {
            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                        UIManager.StartLayer(nameof(HudUI), ParentScene);
                        break;
                }
                
            });
            
            Model = new ChatModel
            {
                TextBoxText = ""
            };
        }
        
        public override void Body()
        {

            
            RootUI.Background(Color.Transparent);
            
            RootUI.Vstack(v1 =>
                {
                    v1.ScrollableContainer(s1 =>
                    {
                        s1.ListFor(Model, x => x.Messages, (stack, item) =>
                        {
                            stack.Hstack(h1 => { h1.Text(item).FontSize(0.5f); })
                                .Height(0.07f);
                        });
                        
                    }).Grow(12);
                    

                    v1.TextBoxFor(Model, x => x.TextBoxText)
                        .Background(new Color(0,0,0,0.5f))
                        .Color(Color.White);

                })
                    .Width(0.4f)
                    .Height(0.7f)
                    .Bottom(0)
                    .Right(0);
        }
    }

    public class ChatModel
    {
        public const int MaxMessages = 8;
        
        public string TextBoxText { get; set; }


        public List<string> Messages;

        public ChatModel()
        {
            Messages = new List<string> {"Madmax: Test","Madmax: Test1Test1 Test1Test1 Test1 Test1Test1 Test1 Test1 Test1 Test1Test1 Test1 Test1 Test1 Test1 Test1","Madmax: Test2","Madmax: Test3","Madmax: Test4","Madmax: Test5"};
        }
    }
}