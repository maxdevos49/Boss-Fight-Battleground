using System;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.UI;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.UI
{
    public class HudUI : UILayer
    {
        
        private ClientInventory Inventory { get; set; }
        
        private  ClientEntity ClientEntity { get; set; }

        public HudUI() : base(nameof(HudUI))
        {
            BlockInput = false;
        }

        protected override void Init()
        {
            
            Inventory = ClientDataRegistry.GetInstance().Inventory;
            ClientEntity = ClientDataRegistry.GetInstance().Client;

            #region Keypress Events

            AddInputListener("keypress", e =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Escape:
                        UIManager.StartLayer(nameof(GameMenuUI),ParentScene);
                        break;
                    case Keys.E:
                        UIManager.LaunchLayer(nameof(InventoryUI), ParentScene);
                        break;
                    case Keys.T:
                        UIManager.StartLayer(nameof(ChatUI), ParentScene);
                        break;
                    case Keys.D1:
                        Inventory.ActiveSlot = 0;
                        break;
                    case Keys.D2:
                        Inventory.ActiveSlot = 1;
                        break;
                    case Keys.D3:
                        Inventory.ActiveSlot = 2;
                        break;
                    case Keys.D4:
                        Inventory.ActiveSlot = 3;
                        break;
                    case Keys.D5:
                        Inventory.ActiveSlot = 4;
                        break;
                    case Keys.D6:
                        Inventory.ActiveSlot = 5;
                        break;
                    case Keys.D7:
                        Inventory.ActiveSlot = 6;
                        break;
                }
            });

            #endregion
            
            #region MouseScroll Events
            
            AddInputListener("mousescroll", e =>
            {

                if (Math.Abs(e.Mouse.VerticalScrollAmount) <= 200 || e.Keyboard.KeyboardState.IsKeyDown(Keys.LeftControl))
                    return;
                
                if (e.Mouse.VerticalScrollAmount > 0)
                    Inventory.ActiveSlot++;
                else
                {
                    if (Inventory.ActiveSlot == 0)
                        Inventory.ActiveSlot = 6;
                    else
                        Inventory.ActiveSlot--;
                }

                if (Inventory.ActiveSlot > 6)
                    Inventory.ActiveSlot = 0;

            });
            
            #endregion
        }

        public override void Body()
        {

            RootUI.Background(Color.Transparent);

            RootUI.Zstack(z1 =>
            {
                z1.Hstack(h2 =>
                    {
                        h2.Text = ClientEntity?.Meta != null ? ClientEntity.Meta.Health.ToString() : "AHAHAHA";

                        //Health Bar

                        //Mana Bar
                    }).Height(0.7f)
                    .Width(0.7f)
                    .Center(); ;
                
                //Hotbar
                z1.Hstack(h2 =>
                    {
                        for (byte i = 0; i < 7; i++)
                        {
                            h2.InventorySlot(Inventory, i, hotBarMode:true, clickAction: (e, slotId) =>
                            {
                                if (e.Mouse.LeftButton == ButtonState.Pressed)
                                    Inventory.ActiveSlot = slotId;
                            });
                        }
                    })
                    .Width(0.45f)
                    .Height(0.1f)
                    .Center()
                    .Bottom(0);
            });

        }
    }
}