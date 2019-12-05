using System.Collections.Generic;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class InventoryUI : UILayer
    {
        public InventoryUI() : base(nameof(InventoryUI))
        {
            BlockInput = true;
            List = new List<int> {10,45,12,423,23,11,44,23,546,12,53,1212,434,23,121,545,6757};
        }
        
        private List<int> List { get; set; }

        protected override void Init()
        {
            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                    case Keys.E:
                        UIManager.StopLayer(Key);
                        break;
                }
            });
        }
        
        public override void Body()
        {
            RootUI.Vstack(v1 =>
            {
                v1.Button("Close", clickAction: (e, a) =>
                {
                    UIManager.StopLayer(Key); //Stop this layer
                }).Image("button");
                
                v1.Spacer();
                v1.Spacer();
                
                v1.ListFor(this, x => x.List, (stack, item) => { 
                    
                    stack.Hstack(h1 =>
                    {
                        h1.Spacer(1);
                        h1.Text("Test" + item);
                        h1.Button("Test" + item)
                            .Image("button");
                    }).Width(0.3f); 
                    
                },StackDirection.Horizontal)
                    .Background(Color.Beige);

            });

        }
    }
}