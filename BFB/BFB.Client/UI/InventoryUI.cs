using BFB.Engine.Inventory;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class InventoryUI : UILayer
    {
        
        private ClientInventory Inventory { get; set; }
        
        public InventoryUI() : base(nameof(InventoryUI))
        {
            BlockInput = true;
        }
        
        protected override void Init()
        {
            
            Inventory = ClientDataRegistry.GetInstance().Inventory;

         
            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                    case Keys.E:
                        UIManager.StopLayer(Key);
                        break;
                }
                e.StopPropagation();
            });
        }
        
        public override void Body()
        {
            
            RootUI.Background(Color.Transparent);
            
            RootUI.Hstack(h1 =>
                {
                    h1.Spacer();

                    h1.Vstack(v2 =>
                        {

                            for (int i = 3; i > -1; i--)
                            {
                                if (i == 0)
                                    v2.Spacer(2);
                                
                                int i2 = i;
                                v2.Hstack(h2 =>
                                {
                                    for (int j = 0; j < 7; j++)
                                    {
                                        int i1 = i2;
                                        int j1 = j;
                                        
                                        h2.InventorySlot(Inventory, i1 * 7 + j1, clickAction: (e, slotId) =>
                                        {
                                        
                                        });
                                    }
                                })
                                    .Grow(10);
                                
                            }
                            
                        })
                        .AspectRatio(1.78f)
                        .Center()
                        .Grow(4);
                    
                    h1.Spacer(); 
                })
                .Background(new Color(0,0,0,0.4f));

        }
    }
}