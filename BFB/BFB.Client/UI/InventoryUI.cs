using System.Collections.Generic;
using BFB.Engine.Inventory;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class InventoryUI : UILayer
    {
        
        public ClientInventory Inventory { get; set; }
        
        public InventoryUI() : base(nameof(InventoryUI))
        {
            Debug = true;
            BlockInput = true;

            Inventory = new ClientInventory
            {
                InventorySize = 24,
                HotBarRange = 5
            };

            var newSlot = new InventorySlot
            {
                Count = 20,
                Name = "Dirt Blocks",
                Mode = false,
                SlotId = 1,
                TextureKey = "Tiles:Dirt"
            };
            
            Inventory.AddItem(1, newSlot);
        }
        
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
                e.StopPropagation();
            });
        }
        
        public override void Body()
        {
            RootUI.Hstack(h1 =>
                {
                    h1.Spacer();

                    h1.Vstack(v2 =>
                        {

                            for (int i = 0; i < 27; i+=7)
                            {
                                int i1 = i;

                                v2.Hstack(h2 =>
                                {
                                    h2.InventorySlot(Inventory, i1);
                                    h2.InventorySlot(Inventory, i1 + 1);
                                    h2.InventorySlot(Inventory, i1 + 2);
                                    h2.InventorySlot(Inventory, i1 + 3);
                                    h2.InventorySlot(Inventory, i1 + 4);
                                    h2.InventorySlot(Inventory, i1 + 5);
                                    h2.InventorySlot(Inventory, i1 + 6);
                                });
                            }
                            
                        })
                        .AspectRatio(1.78f)
                        .Center()
                        .Grow(3);
                    
                    h1.Spacer(); 
                })
                .Background(new Color(0,0,0,0.4f));

        }
    }
}